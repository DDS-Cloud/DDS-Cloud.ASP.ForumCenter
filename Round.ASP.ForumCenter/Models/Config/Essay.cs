using System;

namespace Round.ASP.ForumCenter.Models.Config
{
    public class Essay
    {
        public class EssayClass
        {
            public string ESID { get; set; }
            public string MessTitle { get; set; }
            public string MessMessage { get; set; }
            public string UserUUID { get; set; }
            public string CreatTime { get; set; }
        }
        public static void AddEssayWithUUID(string uuid,string mess,string tit)
        {
            var te = new EssayClass
            {
                ESID = GetNewESID(),
                MessTitle = tit,
                MessMessage = mess,
                UserUUID = uuid,
                CreatTime = DateTime.Now.ToString(),
            };

            foreach(var it in OneUserConfig.Users)
            {
                if (it.uuid == uuid)
                {
                    it.Essay.Add(te);
                }
            }

            OneUserConfig.SaveUserConfigForUUID(uuid);
            Task.Run(() =>
            {
                ConfigCore.LoadingConfig();
            });
        }
        public static string GetNewESID()
        {
            string result = "";
            string str = "ABCDEFGHIJKLMNOPQRSTUVWUXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 19; i++)
            {
                result += str[new Random().Next(0, str.Length - 1)];
            }
            return result;
        }
        public static EssayClass GetEssayForESID(string esid)
        {
            foreach(var it in OneUserConfig.Users)
            {
                foreach(var te in it.Essay)
                {
                    if (te.ESID == esid) return te;
                }
            }
            return new EssayClass();
        }
    }
}
