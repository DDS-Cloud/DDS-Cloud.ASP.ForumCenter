using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Round.ASP.ForumCenter.Pages.User
{
    public class IndividualModel : PageModel
    {
        public string uuid { get; set; } = null;
        public void OnGet(string uuid = null)
        {
            if(uuid != null)
            {
                this.uuid = uuid;
            }
            else
            {
                Response.Redirect($"/User/Login");
            }
        }
    }
}
