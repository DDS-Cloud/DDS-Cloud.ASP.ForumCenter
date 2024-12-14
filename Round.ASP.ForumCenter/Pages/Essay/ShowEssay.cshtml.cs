using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Round.ASP.ForumCenter.Pages.Essay
{
    public class ShowEssayModel : PageModel
    {
        public string esid {  get; set; }
        public void OnGet(string esid = null)
        {
            this.esid = esid;
        }
    }
}
