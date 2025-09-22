using Microsoft.AspNetCore.Mvc;

namespace SocPass.UI.Controllers
{
    public class MemberDetailsController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> IsVisited(int memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                return BadRequest("Member Id is not found.");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            long.TryParse(userIdClaim, out long loggedInUserId);
            bool success = await _showTranscriptServices.MarkIsTranscriptAsync(filename, loggedInUserId);
            if (success)
            {
                return RedirectToAction("ShowTranscript", "ShowTranscript");
            }
            else
            {
                TempData["Error"] = "File not found or already marked.";
                return RedirectToAction("FileList", "ShowTranscript");
            }
        }
    }
}
