using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Round.ASP.ForumCenter.Pages
{
    public class IndexModel : PageModel
    {
        public string uuid { get; set; } = null;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string uuid = null)
        {
            this.uuid = uuid;
        }
    }
}
