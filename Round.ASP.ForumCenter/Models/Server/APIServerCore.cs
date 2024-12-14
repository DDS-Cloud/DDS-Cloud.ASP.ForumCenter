using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Round.ASP.ForumCenter.Models.Config;

namespace Round.ASP.ForumCenter.Models.Server
{
    public class APIServerCore
    {
        public static void StartServerAsync()
        {
            int port = ConfigCore.MainConfig.APIPort; // 选择一个端口号
            TcpServer server = new TcpServer(port);

            Console.CancelKeyPress += (sender, eArgs) =>
            {
                eArgs.Cancel = true; // 阻止进程退出
                server.Stop();
                Console.WriteLine("Server stopped.");
            };

            server.Start();
        }

        class TcpServer
        {
            private TcpListener _listener;
            private bool _running;

            public TcpServer(int port)
            {
                _listener = new TcpListener(IPAddress.Any, port);
            }

            public void Start()
            {
                _listener.Start();
                _running = true;

                Console.WriteLine($"Server started on port {_listener.LocalEndpoint}.");

                while (_running)
                {
                    Task<TcpClient> clientTask = _listener.AcceptTcpClientAsync();
                    clientTask.Wait(); // 等待客户端连接
                    TcpClient client = clientTask.Result;

                    // 为每个客户端连接创建一个新任务
                    Task.Run(() => HandleClientAsync(client));
                }
            }

            private async Task HandleClientAsync(TcpClient client)
            {
                try
                {
                    NetworkStream stream = client.GetStream();

                    byte[] buffer = new byte[99999999];
                    int bytesRead;
                    StringBuilder messageData = new StringBuilder();

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageData.Append(message);

                        int dataIndex = messageData.ToString().IndexOf("data=");
                        if (dataIndex != -1)
                        {
                            // 提取data=后面的内容
                            string jsonData = messageData.ToString().Substring(dataIndex + 5 /* "data=".Length */);
                            //Console.WriteLine("JSON Data: " + jsonData);

                            Console.WriteLine(jsonData);
                            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData.Replace("\\", "\\\\").Replace("\n", "\\n"));
                            string type = data["type"];

                            switch(type)
                            {
                                case "AddEssay":
                                    string Mess = data["Mess"];
                                    string Tit = data["Title"];
                                    string uuid = data["UUID"];

                                    Console.WriteLine("内容："+ Mess);
                                    Console.WriteLine("标题：" + Tit);
                                    Console.WriteLine("用户统一通行证：" + uuid);   //有问题在群里问我

                                    Essay.AddEssayWithUUID(uuid, Mess, Tit);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Data not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                }
                finally
                {
                    // 关闭客户端连接
                    client.Close();
                }
            }

            public void Stop()
            {
                _running = false;
                _listener.Stop();
            }
        }

    }
}
