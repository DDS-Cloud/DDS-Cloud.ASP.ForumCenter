using System.Globalization;

namespace Round.ASP.ForumCenter.Models.More
{
    public class GetRandomTitle
    {
        public static List<string> Titles = new List<string> { 
            "等灯云讨论中心",
            "huh?等灯云杂货站?",
            "你想要的杂牌文章这里都有!",
            "云服务器，就选等灯云",
            "页面不见啦！(doge)",
            "等灯云每个月至少亏300",
            "给我们赞助可以体验最新内测的内容哦！"
        };
        public static string GetTitle()
        {
            return Titles[new Random().Next(0,Titles.Count)];
        }
    }
}
