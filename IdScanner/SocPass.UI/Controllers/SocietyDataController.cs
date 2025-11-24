using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using System.Security.Claims;
using SocPass.UI.Filters;
namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
    public class SocietyDataController : Controller
    {
        private readonly ISocietyDataService _societyDataService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly GlobalClass _globalClass;

        public SocietyDataController(
            ISocietyDataService societyDataService,
            IBlockService blockService,
            ISocietyService societyService,
            IFlatService flatService,
            ISubscriptionService subscriptionService,GlobalClass globalClass)
        {
            _societyDataService = societyDataService;
            _blockService = blockService;
            _societyService = societyService;
            _flatService = flatService;
            _subscriptionService = subscriptionService;
            _globalClass = globalClass;
        }
        public async Task<IActionResult> SocietyDataList()
        {
            IEnumerable<SocietyData> SocietyDataList = await _societyDataService.GetSocietyDataAsync();
            ViewBag.SocietyDataList = SocietyDataList;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddSocietyData()
        {
            bool isAdmin = User.IsInRole("Admin");

            var societies = await _societyService.GetSocietyAsync();

            int selectedSocietyId = 0;

            if (!isAdmin)
            {
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View(new SocietyDataCreateRequest());
        }
        [HttpPost]
        public async Task<IActionResult> AddSocietyData([FromBody] SocietyDataCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }
            try
            {
                var message = await _societyDataService.AddSocietyDataAsync(request);

                if (message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, message = "Society Data Already exists" });
                }

                return Json(new { success = true, message = "Society Data added successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSocietyData(int id)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var societyData = await _societyDataService.GetSocietyDataByIdAsync(id);
            if (societyData == null)
                return NotFound();

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            ViewBag.Societies = await _societyService.GetSocietyAsync();
            if (societyData.Flat?.Block?.SocietyId != null)
            {
                ViewBag.Blocks = await _blockService.GetBlockBySocietyId(societyData.Flat.Block.SocietyId);
            }
            if (societyData.FlatId != 0)
            {
                ViewBag.Flats = await _flatService.GetFlatByBlockId(societyData.Flat.BlockId);
            }
            ViewBag.SavedSocietyId = societyData.Flat?.Block?.SocietyId ?? 0;
            ViewBag.SavedBlockId = societyData.Flat?.BlockId ?? 0;
            ViewBag.SavedFlatId = societyData.FlatId;

            return View(societyData);
        }


        [HttpPost]
        public async Task<IActionResult> EditSocietyData([FromBody] SocietyData request)
        {
            if (ModelState.IsValid && request.SocietyDataId > 0)
            {
                await _societyDataService.UpdateSocietyDataAsync(request);
                return Ok();
            }
            return BadRequest("Invalid update data");
        }

        // DELETE
        public async Task<IActionResult> DeleteSocietyData(int id)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            await _societyDataService.DeleteSocietyDataAsync(id);
            return RedirectToAction("SocietyDataList");
        }

        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            return Json(blocks.Select(b => new { blockId = b.BlockId, blockName = b.BlockNumber }));
        }

        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId)
        {
            var flats = await _flatService.GetFlatByBlockId(blockId);
            return Json(flats.Select(f => new { flatId = f.FlatId, flatNumber = f.FlatNumber }));
        }
        [HttpGet]
        public async Task<IActionResult> GetSubscriptionBySociety(int societyId)
        {
            var subscription = await _subscriptionService.GetSubscriptionBySocietyIdAsync(societyId);
            if (subscription == null)
            {
                return Json(new { allowsName = 0, allowsNumber = 0, allowsEmail = 0 });
            }
            return Json(new
            {
                allowsName = subscription.AllowNoOfName,
                allowsNumber = subscription.AllowNoOfContact,
                allowsEmail = subscription.AllowNoOfEmail
            });
        }

    }
}


