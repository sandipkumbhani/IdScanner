using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class FlatController : Controller
    {
        private readonly IFlatService _flatRepository;
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private GlobalClass _globalClass;
        public FlatController(IFlatService flatRepository, GlobalClass globalClass, ISocietyService societyService, IBlockService blockService)
        {
            _flatRepository = flatRepository
                ?? throw new ArgumentNullException(nameof(flatRepository));

            _societyService = societyService
                ?? throw new ArgumentNullException(nameof(societyService));
            _globalClass = globalClass;
            _blockService = blockService;
        }

        //public async Task<IActionResult> FlatList()
        //{
        //    //if (string.IsNullOrEmpty(_globalClass.Token))
        //    //{
        //    //    return RedirectToAction("Login", "Login");
        //    //}
        //    IList<Flat> FlatList = await _flatRepository.GetAllFlatAsync();
        //    ViewBag.FlatList = FlatList;
        //    return View("~/Views/Flat/FlatList.cshtml");
        //}
        //[HttpGet]
        //public async Task<IActionResult> AddFlat(int? societyId, int blockId)
        //{
        //    //if (string.IsNullOrEmpty(_globalClass.Token))
        //    //{
        //    //    return RedirectToAction("Login", "Login");
        //    //}
        //    var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
        //    int.TryParse(userIdClaim, out int userId);
        //    var societies = await _societyService.GetAllSocietyAsync(userId);
        //    ViewBag.SocietyList = societies;


        //    if (societyId == null || blockId == 0)
        //    {
        //        return View(new Flat());
        //    }
        //    var flat = await _flatRepository.GetFlatByIdAsync(societyId, blockId);
        //    return View(flat);
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddFlat(Flat flat)
        //{
        //    //if (string.IsNullOrEmpty(_globalClass.Token))
        //    //{
        //    //    return RedirectToAction("Login", "Login");
        //    //}
        //    var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
        //    int.TryParse(userIdClaim, out int userId);
        //    var societies = await _societyService.GetAllSocietyAsync(userId);
        //    ViewBag.SocietyList = societies;

        //    await _flatRepository.UpdateFlatAsync(flat);

        //    return RedirectToAction("FlatList");
        //}

        public async Task<IActionResult> FlatList()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            var flatList = (await _flatRepository.GetAllFlatAsync()).ToList();

            ViewBag.FlatList = flatList;
            ViewBag.IsAdmin = User.IsInRole("Admin");

            // Pass user's society name if not admin
            if (!User.IsInRole("Admin"))
            {
                var societies = await _societyService.GetAllSocietyAsync(userId);
                var userSocietyName = societies.FirstOrDefault()?.Name ?? "";
                ViewBag.UserSocietyName = userSocietyName;
            }
            else
            {
                ViewBag.UserSocietyName = ""; // optional
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddFlat(int? societyId, int blockId)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            // Admin = access all societies, else user-specific
            if (User.IsInRole("Admin"))
                societies = await _societyService.GetAllSocietyAsync(); // all societies
            else
                societies = await _societyService.GetAllSocietyAsync(userId); // only user's society

            Flat flat;

            // ---------- NEW Flat ----------
            if (societyId == null || blockId == 0)
            {
                flat = new Flat();

                if (!User.IsInRole("Admin"))
                {
                    // Non-admin → prefill and lock the dropdown
                    var assignedSociety = societies.FirstOrDefault();
                    if (assignedSociety != null)
                        flat.SocietyId = assignedSociety.SocietyId;

                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
                    ViewBag.IsSocietyReadonly = true;
                }
                else
                {
                    // Admin → editable dropdown
                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name");
                    ViewBag.IsSocietyReadonly = false;
                }
                return View(flat);
            }

            // ---------- EDIT Flat ----------
            var flatList = await _flatRepository.GetFlatByIdAsync(societyId, blockId);
            flat = flatList.FirstOrDefault() ?? new Flat();
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");
            return View(flat);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlat(Flat flat)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
            {
                // Admin → all societies
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                // Non-admin → only assigned society
                var assignedSociety = await _societyService.GetAllSocietyAsync(userId);
                societies = assignedSociety; // assuming method returns IEnumerable<Society>
                flat.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0; // enforce assigned society
            }

            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", flat.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            await _flatRepository.UpdateFlatAsync(flat); // existing flat
            return RedirectToAction("FlatList");
        }


        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(d => new
            {
                blockId = d.BlockId,
                blockNumber = d.BlockNumber
            });
            return Json(result);
        }
    }
}
