using Microsoft.AspNetCore.Mvc;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class MemberDetailsController : Controller
    {
        private readonly IMemberDetailsService _memberDetailsService;
        private readonly IMemberService _memberService;
        private readonly GlobalClass _globalClass;

        public MemberDetailsController(
            IMemberDetailsService memberDetailsService,
            IMemberService memberService,
            GlobalClass globalClass)
        {
            _memberDetailsService = memberDetailsService;
            _memberService = memberService;
            _globalClass = globalClass;
        }

        [HttpGet("MemberDetails/GetDetails/{memberId}/{EventId}")]
        public async Task<IActionResult> GetDetails(int memberId, int EventId)
        {
            if (memberId == 0)
            {
                return BadRequest("Member Id is not found.");
            }

            var result = await _memberService.GetMemberByMemberId(memberId, EventId);
            if (result == null)
            {
                return NotFound();
            }
            string? Message = string.Empty;
            if (result.Visited && TempData["Message"] == null)
                Message = "This member has already visited.";
            else
                Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            ViewBag.Message = Message;
            ViewBag.AlertType = TempData["AlertType"] == null ? "danger" : TempData["AlertType"];

            return View("~/Views/MemberDetails/MemberDetails.cshtml", result);
        }
        [HttpPost]
        public async Task<IActionResult> IsVisited(int memberId)
        {
            if (memberId == 0)
            {
                return BadRequest("Member Id is not found.");
            }

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int loggedInUserId);

            bool success = await _memberDetailsService.IsVisitedAsync(memberId, loggedInUserId);

            TempData["Message"] = success
                ? "Member marked as visited successfully."
                : "Something went wrong. Please try again.";
            TempData["AlertType"] = success ? "success" : "danger";

            return RedirectToAction(nameof(GetDetails), new { memberId });
        }

    }
}
