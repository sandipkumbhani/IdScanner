using Microsoft.AspNetCore.Mvc;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class MemberDetailsController : Controller
    {
        private readonly IMemberDetailsService _memberDetailsService;
        private readonly IMemberService _memberService;
        private GlobalClass _globalClass;
        public MemberDetailsController(IMemberDetailsService memberDetailsService, IMemberService memberService, GlobalClass globalClass)
        {
            _memberDetailsService = memberDetailsService;
            _memberService = memberService;
            _globalClass = globalClass;
        }
        [HttpGet("MemberDetails/GetDetails/{memberId}")]
        public async Task<IActionResult> GetDetails(int memberid)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            var result = await _memberService.GetMemberByMemberId(memberid);
            return View("~/Views/MemberDetails/MemberDetails.cshtml", result);
        }


        [HttpPost]
        public async Task<IActionResult> IsVisited(int memberid)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            if (memberid == 0)
            {
                return BadRequest("Member Id is not found.");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int loggedInUserId);
            bool success = await _memberDetailsService.IsVisitedAsync(memberid, loggedInUserId);
            if (success)
            {
                ViewBag.Message = "Member marked as visited successfully.";
                ViewBag.AlertType = "success";
            }
            else
            {
                ViewBag.Message = "Something went wrong. Please try again.";
                ViewBag.AlertType = "danger";
            }
            return View("MemberDetails");
        }
    }
}
