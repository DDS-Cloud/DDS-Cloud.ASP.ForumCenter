using Round.ASP.ForumCenter.Models.Config;

namespace Round.ASP.ForumCenter.Models.User
{
    public class UserCore
    {
        public class UserConfig
        {
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public string UserEmail { get; set; }
            public string UUID { get; set; }
            public string DSID { get; set; }
        }
        public class UserCaptcha
        {
            public UserConfig UserConfig { get; set; }
            public string Captcha { get; set; }
        }
        public class Register
        {
            public static List<UserCaptcha> Captchas = new List<UserCaptcha>();
            public static bool RegisterUser(string EMail,string UserName,string UserPassWord)
            {
                if (EmailDuplicate(EMail)) return false;
                var Cap = GetNewCaptcha();

                EMailServer.SendEmail(EMail, "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n<meta charset=\"UTF-8\">\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n<title>Phone Number with Copy Button</title>\r\n<style>\r\n  body {\r\n    font-family: Arial, sans-serif;\r\n    display: flex;\r\n    justify-content: center;\r\n    align-items: center;\r\n    height: 100vh;\r\n    margin: 0;\r\n    background-color: #f7f7f7;\r\n  }\r\n  .number-container {\r\n    display: flex;\r\n    flex-direction: column;\r\n    align-items: center;\r\n    background-color: #ffffff;\r\n    padding: 20px;\r\n    border: 1px solid #ddd;\r\n    box-shadow: 0 2px 5px rgba(0,0,0,0.2);\r\n    width: fit-content;\r\n  }\r\n  .phone-number {\r\n    font-size: 24px;\r\n    margin-bottom: 10px;\r\n  }\r\n  .copy-button {\r\n    cursor: pointer;\r\n    padding: 10px 20px;\r\n    background-color: #007bff;\r\n    color: white;\r\n    border: none;\r\n    border-radius: 5px;\r\n    font-size: 16px;\r\n  }\r\n  .copy-button:hover {\r\n    background-color: #0056b3;\r\n  }\r\n</style>\r\n</head>\r\n<body>\r\n\r\n<div class=\"number-container\">\r\n    <label>等灯云 - 邮件服务</label>\r\n <h1>以下是您的验证码</h1>\r\n  <div class=\"phone-number\" id=\"phoneNumber\">"+Cap+"</div>\r\n  <label>请在 5 分钟内输入验证码</label>\r\n  <label>如非本人操作，请忽略此邮件</label>\r\n</div>\r\n\r\n</body>\r\n</html>",true);

                Captchas.Add(new UserCaptcha
                {
                    UserConfig = new UserConfig { 
                        UserEmail = EMail,
                        UserName = UserName,
                        UserPassword = UserPassWord
                    },
                    Captcha = Cap
                });
                return true;
            }
            public static string GetNewUUID()
            {
                string result = "";
                string str = "ABCDEFGHIJKLMNOPQRSTUVWUXYZabcdefghijklmnopqrstuvwxyz0123456789";
                for(int i = 0; i < 19; i++)
                {
                    result += str[new Random().Next(0, str.Length - 1)];
                }
                return result;
            }
            public static bool RegisterForCaptcha(string Cap)
            {
                foreach(var C in Captchas)
                {
                    if(C.Captcha == Cap)
                    {
                        var te = C.UserConfig;
                        te.UUID = GetNewUUID();
                        te.DSID = ConfigCore.MainConfig.UserCount.ToString();
                        ConfigCore.MainConfig.UserCount++;
                        ConfigCore.MainConfig.UserConfig.Add(te); 
                        Captchas.Remove(C);
                        ConfigCore.SaveConfig();
                        OneUserConfig.InitUserConfig(te.UUID);

                        return true;
                    }
                }

                return false;
            }
            public static string GetNewCaptcha()
            {
                var Temp = "";
                for (int i = 0; i <= 7; i++)
                {
                    Temp += new Random().Next(0, 9).ToString();
                }

                if (!CaptchaDuplicates(Temp)) return GetNewCaptcha();
                else return Temp;
            }
            public static bool CaptchaDuplicates(string Captcha)
            {
                foreach (var item in Captchas)
                {
                    if (item.Captcha == Captcha) return false;
                }
                return true;
            }
        }
        public class Login
        {
            public static bool LoginUser(string EMail, string UserPassWord)
            {
                foreach(var us in ConfigCore.MainConfig.UserConfig)
                {
                    if (us.UserEmail == EMail)
                    {
                        if (us.UserPassword == UserPassWord) return true;
                        else return false;
                    };
                }

                return false;
            }
        }

        public static string GetUUIDForEMail(string EMail)
        {
            foreach (var us in ConfigCore.MainConfig.UserConfig)
            {
                if (us.UserEmail == EMail) return us.UUID;
            }
            return null;
        }
        public static bool EmailDuplicate(string EMail)
        {
            foreach (var us in ConfigCore.MainConfig.UserConfig)
            {
                if (us.UserEmail == EMail) return true;
            }
            return false;
        }
        public static UserConfig GetUserConfigForUUID(string uuid)
        {
            foreach (var item in ConfigCore.MainConfig.UserConfig)
            {
                if (item.UUID == uuid) return item;
            }

            return null;
        }
    }
}
