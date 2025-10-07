using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class SocietyDataController : Controller
    {
        private readonly ISocietyDataService _societyDataService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatService;
        private readonly ISubscriptionService _subscriptionService;

        public SocietyDataController(
            ISocietyDataService societyDataService,
            IBlockService blockService,
            ISocietyService societyService,
            IFlatService flatService,
            ISubscriptionService subscriptionService)
        {
            _societyDataService = societyDataService;
            _blockService = blockService;
            _societyService = societyService;
            _flatService = flatService;
            _subscriptionService = subscriptionService;
        }

        // LIST
        public async Task<IActionResult> SocietyDataList()
        {
            var societyDataList = await _societyDataService.GetAllSocietyDataAsync();
            return View(societyDataList);
        }

        // ADD
        [HttpGet]
        public async Task<IActionResult> AddSocietyData()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            ViewBag.Societies = await _societyService.GetAllSocietyAsync(userId);
            return View(new SocietyDataCreateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> AddSocietyData([FromBody] SocietyDataCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                await _societyDataService.AddSocietyDataAsync(request);
                return Ok();
            }
            return BadRequest("Invalid data");
        }

        // EDIT
        [HttpGet]
        public async Task<IActionResult> EditSocietyData(int id)
        {
            var societyData = await _societyDataService.GetSocietyDataByIdAsync(id);
            if (societyData == null)
                return NotFound();

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            ViewBag.Societies = await _societyService.GetAllSocietyAsync(userId);
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
            await _societyDataService.DeleteSocietyDataAsync(id);
            return RedirectToAction("SocietyDataList");
        }

        // AJAX HELPERS
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
                return Json(new { allowsName = 0, allowsNumber = 0, allowsEmail = 0 });

            return Json(new
            {
                allowsName = subscription.AllowNoOfName,
                allowsNumber = subscription.AllowNoOfContact,
                allowsEmail = subscription.AllowNoOfEmail
            });
        }

    }
}


