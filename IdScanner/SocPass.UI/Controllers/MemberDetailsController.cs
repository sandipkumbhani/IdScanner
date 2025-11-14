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
                TempData["Message"] = "Member ID is not valid.";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Error", "Home");
            }

            var result = await _memberService.GetMemberByMemberId(memberId, EventId);
            if (result == null)
            {
                TempData["Message"] = "Member or event details not found.";
                TempData["AlertType"] = "danger";
                return RedirectToAction("Error", "Home");
            }

            string message;
            if (result.Visited && TempData["Message"] == null)
            {
                message = "This member has already visited.";
                ViewBag.AlertType = "info";
            }
            else
            {
                message = TempData["Message"]?.ToString() ?? string.Empty;
                ViewBag.AlertType = TempData["AlertType"]?.ToString() ?? "secondary";
            }
            ViewBag.Message = message;
            return View("~/Views/MemberDetails/MemberDetails.cshtml", result);
        }

        [HttpPost]
        public async Task<IActionResult> IsVisited(int memberId, int EventId)
        {
            if (memberId == 0)
            {
                return BadRequest("Member Id is not found.");
            }

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int loggedInUserId);

            var result = await _memberDetailsService.IsVisitedAsync(memberId, EventId, loggedInUserId);

            switch (result)
            {
                case "Success":
                    TempData["Message"] = "Member marked as visited successfully.";
                    TempData["AlertType"] = "success";
                    break;
                case "AlreadyVisited":
                    TempData["Message"] = "Member has already been marked as visited.";
                    TempData["AlertType"] = "info";
                    break;

                case "NotToday":
                    TempData["Message"] = "You can only mark as visited on the event date.";
                    TempData["AlertType"] = "warning";
                    break;

            }

            return RedirectToAction(nameof(GetDetails), new { memberId = memberId, EventId = EventId });

        }

    }
}


//using Microsoft.AspNetCore.Mvc;
//using SocPass.UI.Application.Interface;
//using SocPass.UI.Domain.Model;
//using System.Text;

//namespace SocPass.UI.Controllers
//{
//    public class MemberDetailsController : Controller
//    {
//        private readonly IMemberDetailsService _memberDetailsService;
//        private readonly IMemberService _memberService;
//        private readonly GlobalClass _globalClass;

//        public MemberDetailsController(
//            IMemberDetailsService memberDetailsService,
//            IMemberService memberService,
//            GlobalClass globalClass)
//        {
//            _memberDetailsService = memberDetailsService;
//            _memberService = memberService;
//            _globalClass = globalClass;
//        }

//        [HttpGet("MemberDetails/GetDetails/{encoded}")]
//        public async Task<IActionResult> GetDetails(string encoded)
//        {
//            try
//            {
//                // Decode Base64 string
//                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
//                var parts = decoded.Split('|');
//                int memberId = int.Parse(parts[0]);
//                int eventId = int.Parse(parts[1]);

//                var result = await _memberService.GetMemberByMemberId(memberId, eventId);
//                if (result == null)
//                    return RedirectToAction("Error", "Home");

//                string message = result.Visited ? "This member has already visited." : TempData["Message"]?.ToString();
//                ViewBag.AlertType = result.Visited ? "info" : TempData["AlertType"]?.ToString() ?? "secondary";
//                ViewBag.Message = message;

//                return View("~/Views/MemberDetails/MemberDetails.cshtml", result);
//            }
//            catch
//            {
//                return RedirectToAction("Error", "Home");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> IsVisited(string encoded)
//        {
//            try
//            {
//                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
//                var parts = decoded.Split('|');
//                int memberId = int.Parse(parts[0]);
//                int eventId = int.Parse(parts[1]);

//                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
//                int.TryParse(userIdClaim, out int loggedInUserId);

//                var result = await _memberDetailsService.IsVisitedAsync(memberId, eventId, loggedInUserId);

//                switch (result)
//                {
//                    case "Success":
//                        TempData["Message"] = "Member marked as visited successfully.";
//                        TempData["AlertType"] = "success";
//                        break;
//                    case "AlreadyVisited":
//                        TempData["Message"] = "Member has already been marked as visited.";
//                        TempData["AlertType"] = "info";
//                        break;
//                    case "NotToday":
//                        TempData["Message"] = "You can only mark as visited on the event date.";
//                        TempData["AlertType"] = "warning";
//                        break;
//                }

//                // Encode again for redirect
//                string reEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{memberId}|{eventId}"));
//                return RedirectToAction(nameof(GetDetails), new { encoded = reEncoded });
//            }
//            catch
//            {
//                return RedirectToAction("Error", "Home");
//            }
//        }
//    }
//}
