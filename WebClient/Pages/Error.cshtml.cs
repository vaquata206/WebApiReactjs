using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages
{
    /// <summary>
    /// Error model
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Request id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Show request id
        /// </summary>
        public bool ShowRequestId
        {
            get
            {
                return !string.IsNullOrEmpty(this.RequestId);
            }
        }

        /// <summary>
        /// On get
        /// </summary>
        public void OnGet()
        {
            this.RequestId = Activity.Current == null ? Activity.Current.Id : HttpContext.TraceIdentifier;
        }
    }
}
