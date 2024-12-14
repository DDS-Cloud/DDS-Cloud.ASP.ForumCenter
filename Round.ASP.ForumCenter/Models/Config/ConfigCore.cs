using Round.ASP.ForumCenter.Models.User;
using static Round.ASP.ForumCenter.Models.User.UserCore;
using System.Text.Json;
using Newtonsoft.Json;

namespace Round.ASP.ForumCenter.Models.Config
{
    public class ConfigCore
    {
        public class Config
        {
            public List<UserCore.UserConfig> UserConfig { get; set;}
            public int UserCount { get; set; }
            public string EMail { get; set; }
            public string EmailToken { get; set; }
            public string EmailServer { get; set; }
            public int APIPort {  get; set; }
            public string Host {  get; set; }
        }
        public static Config MainConfig = new Config();
        public static void LoadingConfig()
        {
            try
            {
                if (File.Exists("Config.json"))
                {
                    MainConfig = System.Text.Json.JsonSerializer.Deserialize<Config>(File.ReadAllText("Config.json"));
                    foreach(var it in MainConfig.UserConfig)
                    {
                        OneUserConfig.LoadUserConfigForUUID(it.UUID);
                    }
                }
                else
                {
                    MainConfig.UserConfig = new List<UserConfig>();
                    MainConfig.UserCount = 0;
                    SaveConfig();
                    LoadingConfig();
                }
            }
            catch
            {
                MainConfig.UserConfig = new List<UserConfig>();
                MainConfig.UserCount = 0;
                SaveConfig();
                LoadingConfig();
            }
        }
        public static void SaveConfig()
        {
            File.WriteAllText("Config.json", System.Text.Json.JsonSerializer.Serialize(MainConfig, new JsonSerializerOptions
            {
                WriteIndented = true // 设置为true以美化JSON
            }));
        }
    }
}
