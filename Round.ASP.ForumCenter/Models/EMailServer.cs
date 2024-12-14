using Round.ASP.ForumCenter.Models.Config;
using System.Net.Mail;

namespace Round.ASP.ForumCenter.Models
{
    public class EMailServer
    {
        public static bool SendEmail(string email, string content,bool html = false)
        {
            string _smtpServer = ConfigCore.MainConfig.EmailServer;   //SMTP服务器
            string _userName = ConfigCore.MainConfig.EMail;   //邮箱
            string _pwd = ConfigCore.MainConfig.EmailToken;   //密码或授权码

            if (_smtpServer == "" || _userName == "" || _pwd == "")
            {
                return false;
            }

            using (System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage())
            {
                /*   
               * msg.To.Add("b@b.com");可以发送给多人   
               */
                msg.To.Add(email);  //设置收件人

                /*  
                * msg.CC.Add("c@c.com");   
                * msg.CC.Add("c@c.com");可以抄送给多人   
                */

                /* 3个参数分别是 发件人地址（可以随便写），发件人姓名，编码*/
                msg.From = new MailAddress(_userName, _userName, System.Text.Encoding.UTF8);


                msg.Subject = "等灯云 - 邮件服务"; //邮件标题   
                msg.SubjectEncoding = System.Text.Encoding.UTF8; //邮件标题编码   
                msg.Body = content;  //邮件内容   
                msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
                msg.IsBodyHtml = html; //是否是HTML邮件   
                msg.Priority = MailPriority.Normal; //邮件优先级

                SmtpClient client = new SmtpClient(_smtpServer, 587); //邮件服务器地址及端口号
                client.EnableSsl = true; //ssl加密发送
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(_userName, _pwd); //邮箱账号  密码
                client.Timeout = 6000;  //6秒超时

                try
                {
                    client.Send(msg);  //发送邮件

                    client.Dispose();  //释放资源
                    return true;
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    return false;
                }
            }
        }
    }
}
