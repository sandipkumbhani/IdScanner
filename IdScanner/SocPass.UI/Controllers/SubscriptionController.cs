using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;
using SocPass.UI.Filters;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private readonly IFlatService _flatService;
        private readonly GlobalClass _globalClass;
        public SubscriptionController(ISubscriptionService subscriptionService, ISocietyService societyService, IBlockService blockService, IFlatService flatService,GlobalClass globalClass)
        {
            _subscriptionService = subscriptionService;
            _societyService = societyService;
            _blockService = blockService;
            _flatService = flatService;
            _globalClass = globalClass;
        }
        public async Task<IActionResult> SubScriptionList()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            var SubScriptionList = await _subscriptionService.GetAllSubscription();
            ViewBag.SubScriptionList = SubScriptionList;
            return View("~/Views/SubScription/SubScriptionList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddSubScription(int? subscriptionId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var societies = await _societyService.GetSocietyByUserId(userId);
            ViewBag.SocietyList = societies;

            Subscription model;
            if (subscriptionId.HasValue && subscriptionId.Value > 0)
            {
                model = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }
            else
            {
                model = new Subscription();
            }
            return View("/Views/SubScription/AddSubScription.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubScription(Subscription subscription)
        {
            if (subscription == null)
            {
                return BadRequest("Invalid data");
            }
            bool exists = await _subscriptionService.ExistsSocietyDataAsync(subscription.SocietyId, subscription.SubscriptionId);
            if (exists)
            {
                var errormessage ="This society already has a subscription.";
                ViewBag.ErrorMessage = errormessage;
                var SocietyList = await _societyService.GetAllSocietyAsync();
                ViewBag.SocietyList = SocietyList;
                return View("AddSubScription", subscription);
            }

            if (!ModelState.IsValid)
            {
                return View("AddSubScription", subscription);
            }
            if (subscription.SubscriptionId == 0)
            {
                await _subscriptionService.AddSubscriptionAsync(subscription);
            }
            else
            {
                await _subscriptionService.UpdateSubscriptionAsync(subscription);
            }
            return RedirectToAction("SubScriptionList","Subscription");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSubscription(int subscriptionId)
        {
            try
            {
                await _subscriptionService.DeleteSubscriptionAsync(subscriptionId);
                return RedirectToAction("SubScriptionList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"User with ID {subscriptionId} not found: {ex.Message}";
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AppSetting(int? subscriptionId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            var societies = await _societyService.GetSocietyByUserId(userId);
            ViewBag.SocietyList = societies;

            Subscription model;
            if (subscriptionId.HasValue && subscriptionId.Value > 0)
            {
                model = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }
            else
            {
                model = new Subscription();
            }

            return View("~/Views/AppSetting/AppSetting.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AppSetting(Subscription subscription)
        {
            if (subscription == null)
            {
                return BadRequest("Invalid data");
            }
            if (!ModelState.IsValid)
            {
                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
                int.TryParse(userIdClaim, out int userId);
                var societies = await _societyService.GetSocietyByUserId(userId);
                ViewBag.SocietyList = societies;

                return View("~/Views/SubScription/AppSetting.cshtml", subscription);
            }

            var existing = await _subscriptionService.GetSubscriptionBySocietyIdAsync(subscription.SocietyId);

            if (existing != null)
            {
                existing.IsActive = true;
                existing.AllowNoOfName = subscription.AllowNoOfName;
                existing.AllowNoOfContact = subscription.AllowNoOfContact;
                existing.AllowNoOfEmail = subscription.AllowNoOfEmail;
                await _subscriptionService.UpdateSubscriptionAsync(existing);
            }

            TempData["SuccessMessage"] = "App settings updated successfully.";
            return RedirectToAction("SubScriptionList", "Subscription");
        }


    }
}
