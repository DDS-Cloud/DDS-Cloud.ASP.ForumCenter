using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Round.ASP.ForumCenter.Models;
using Round.ASP.ForumCenter.Models.User;

namespace Round.ASP.ForumCenter.Pages.User
{
    public class RegisterModel : PageModel
    {
        public bool YSM { get; set; } = false;
        public bool Mess { get; set; } = false;
        public bool CapN { get; set; } = false;
        public void OnGet(string UserName=null,string EMail = null, string PassWord = null,string Captcha=null)
        {
            if (UserName != null && EMail != null && PassWord != null) { 
                YSM = true;
                Task.Run(()=> Models.User.UserCore.Register.RegisterUser(EMail,UserName,PassWord));
            }
            if (Captcha != null) {
                var temp = UserCore.Register.RegisterForCaptcha(Captcha);
                Console.WriteLine(temp);
                if (temp)
                {
                    YSM = true;
                    Mess = true;
                    CapN = true;
                }
                else
                {
                    YSM = true;
                    Mess = true;
                    CapN = false;
                }
            }
        }
    }
}
