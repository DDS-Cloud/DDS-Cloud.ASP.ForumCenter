using System.Text.Json;
using static Round.ASP.ForumCenter.Models.Config.Essay;

namespace Round.ASP.ForumCenter.Models.Config
{
    public class OneUserConfig
    {
        public static List<UserConfig> Users { get; set; } = new List<UserConfig>();
        public class UserConfig
        {
            public string uuid {  get; set; }
            public List<Essay.EssayClass> Essay {  get; set; }
        }
        public static void InitUserConfig(string uuid)
        {
            Directory.CreateDirectory($"UserConfig\\{uuid}");
            var te = new UserConfig { uuid = uuid, Essay = new List<EssayClass>() };
            Users.Add(te);
            SaveUserConfigForUUID(uuid);
            LoadUserConfigForUUID(uuid);
        }
        public static void SaveUserConfigForUUID(string uuid)
        {
            foreach (var user in Users)
            {
                if (user.uuid == uuid)
                {

                    File.WriteAllText($"UserConfig\\{uuid}\\Config.json", System.Text.Json.JsonSerializer.Serialize(user, new JsonSerializerOptions
                    {
                        WriteIndented = true // 设置为true以美化JSON
                    }));
                }
            }
        }
        public static void LoadUserConfigForUUID(string uuid)
        {
            var te = System.Text.Json.JsonSerializer.Deserialize<UserConfig>(File.ReadAllText($"UserConfig\\{uuid}\\Config.json"));
            if (!Users.Contains(te))
            {
                Users.Add(te);
            }
        }
        public static UserConfig GetUserConfig(string uuid)
        {
            foreach(var user in Users)
            {
                if (user.uuid == uuid) {
                    var tem = user;
                    var sortedMessagesDescending = user.Essay.OrderByDescending(m => m.CreatTime).ToList();
                    tem.Essay = sortedMessagesDescending;
                    return tem;
                };
            }
            return null;
        }

        public static List<Essay.EssayClass> GetRandomEssay(int num)
        {
            if (Users == null || Users.Count == 0)
            {
                //throw new InvalidOperationException("Users list is empty.");
                return new List<EssayClass>();
            }

            var Lists = new List<EssayClass>();
            var inm = Users.Count;
            for (int i = 0; i < num - 1; i++)
            {
                if (inm < 0)
                {
                    return new List<EssayClass>();
                }

                var randomUserIndex = new Random().Next(0, inm);
                var userEssays = Users[randomUserIndex].Essay;
                if (userEssays == null || userEssays.Count == 0)
                {
                    return new List<EssayClass>();
                }

                var unm = userEssays.Count;
                if (unm < 0)
                {
                    return new List<EssayClass>();
                }

                var essayIndex = new Random().Next(0, unm);
                var essay = userEssays[essayIndex];

                bool te = false;
                foreach(var iss in Lists)
                {
                    if (iss.ESID == essay.ESID)
                    {
                        te = true;
                    }
                }
                if (te) continue;
                else Lists.Add(essay);
            }

            var sortedMessagesDescending = Lists.OrderByDescending(m => m.CreatTime).ToList();
            return sortedMessagesDescending;
        }
    }
}
