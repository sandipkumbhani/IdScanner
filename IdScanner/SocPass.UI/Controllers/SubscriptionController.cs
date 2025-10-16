using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
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
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var SubScriptionList = await _subscriptionService.GetAllSubscription();
            ViewBag.SubScriptionList = SubScriptionList;
            return View("~/Views/SubScription/SubScriptionList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddSubScription(int? subscriptionId)
        {
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            { 
                return RedirectToAction("AccessDenied", "AccessDenied"); 
            }
            int.TryParse(userIdClaim, out int userId);
            var societies = await _societyService.GetAllSocietyAsync(userId);
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
                ModelState.AddModelError("SocietyId", "This society already has a subscription.");
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
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }

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
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(b => new
            {
                blockId = b.BlockId,
                blockName = b.BlockNumber
            });
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId)
        {
            var flats = await _flatService.GetFlatByBlockId(blockId);
            var result = flats.Select(f => new
            {
                flatId = f.FlatId,
                flatNumber = f.FlatNumber
            });
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> AppSetting(int? subscriptionId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            var societies = await _societyService.GetAllSocietyAsync(userId);
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
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            if (subscription == null)
            {
                return BadRequest("Invalid data");
            }
            if (!ModelState.IsValid)
            {
                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
                int.TryParse(userIdClaim, out int userId);
                var societies = await _societyService.GetAllSocietyAsync(userId);
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
