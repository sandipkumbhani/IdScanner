using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
    public class FlatController : Controller
    {
        private readonly IFlatService _flatService;
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private GlobalClass _globalClass;
        public FlatController(IFlatService flatService, GlobalClass globalClass, ISocietyService societyService, IBlockService blockService)
        {
            _flatService = flatService
                ?? throw new ArgumentNullException(nameof(flatService));

            _societyService = societyService
                ?? throw new ArgumentNullException(nameof(societyService));
            _globalClass = globalClass;
            _blockService = blockService;
        }
        public async Task<IActionResult> FlatList()
        {
            ICollection<Flat> FlatList = await _flatService.GetFlatAsync();
            ViewBag.FlatList = FlatList.ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddFlat()
        {
            bool isAdmin = User.IsInRole("Admin");
            var societies = await _societyService.GetSocietyAsync();

            var flat = new Flat();

            if (!isAdmin)
            {
                flat.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !isAdmin;
            return View(flat);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlat(Flat flat)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies = await _societyService.GetSocietyAsync();
            flat.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            if (!ModelState.IsValid)
            {
                return View(flat);
            }

            try
            {
                await _flatService.UpdateFlatAsync(flat);
                TempData["SuccessMessage"] = "Flat updated successfully.";
                return RedirectToAction("FlatList");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(flat);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An unexpected error occurred while saving the flat.";
                return View(flat);
            }
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
    }
}
