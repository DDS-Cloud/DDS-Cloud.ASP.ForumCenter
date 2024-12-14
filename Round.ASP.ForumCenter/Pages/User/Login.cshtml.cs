using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Round.ASP.ForumCenter.Models.User;

namespace Round.ASP.ForumCenter.Pages.User
{
    public class LoginModel : PageModel
    {
        public bool Login { get; set; } = false;
        public string uuids { get; set; } = null;
        public void OnGet(string EMail = null, string PassWord = null)
        {
            if(EMail!=null && PassWord!=null)
            {
                if(UserCore.Login.LoginUser(EMail, PassWord))
                {
                    var uuid = UserCore.GetUUIDForEMail(EMail);
                    Login = true;
                    uuids = uuid;
                    Response.Redirect($"/Index?uuid={uuid}");
                }
                else
                {
                    Login = true;
                }
            }
        }
    }
}
