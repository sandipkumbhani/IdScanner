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

        [HttpGet("MemberDetails/GetDetails/{memberId}")]
        public async Task<IActionResult> GetDetails(int memberId)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
                return RedirectToAction("Login", "Login");

            if (memberId == 0)
                return BadRequest("Member Id is not found.");

            var result = await _memberService.GetMemberByMemberId(memberId);
            if (result == null)
                return NotFound();

            ViewBag.Message = TempData["Message"];
            ViewBag.AlertType = TempData["AlertType"];

            return View("~/Views/MemberDetails/MemberDetails.cshtml", result);
        }

        [HttpPost]
        public async Task<IActionResult> IsVisited(int memberId)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
                return RedirectToAction("Login", "Login");

            if (memberId == 0)
                return BadRequest("Member Id is not found.");

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int loggedInUserId);

            bool success = await _memberDetailsService.IsVisitedAsync(memberId, loggedInUserId);

            TempData["Message"] = success
                ? "Member marked as visited successfully."
                : "Something went wrong. Please try again.";
            TempData["AlertType"] = success ? "success" : "danger";

            // PRG pattern: reload the updated entity
            return RedirectToAction(nameof(GetDetails), new { memberId });
        }

    }
}
