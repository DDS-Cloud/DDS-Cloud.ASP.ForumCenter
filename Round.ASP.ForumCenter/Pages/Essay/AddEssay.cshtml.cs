using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Round.ASP.ForumCenter.Pages.Essay
{
    public class AddEssayModel : PageModel
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
