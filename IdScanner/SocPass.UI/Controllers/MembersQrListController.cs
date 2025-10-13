using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class MembersQrListController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatRepository;
        public MembersQrListController(IMemberService memberService, IBlockService blockService, ISocietyService societyService, IFlatService flatService)
        {
            _memberService = memberService;
            _blockService = blockService;
            _societyService = societyService;
            _flatRepository = flatService;

        }

        [HttpGet]
        public async Task<IActionResult> MemberQrList()
        {
            //var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            //int.TryParse(userIdClaim, out int userId);
            //var societies = await _societyService.GetAllSocietyAsync(userId);
            //ViewBag.Societies = societies;
            //return View("/Views/MembersQrList/MembersQrList.cshtml");

            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                // Admin sees all societies
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
            }
            else
            {
                // User sees only assigned societies
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;

            return View("/Views/MembersQrList/MembersQrList.cshtml");

        }
        //[HttpPost]
        //public async Task<IActionResult> GeneratePass(int blockId, DateTime passDate)
        //{
        //    //var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
        //    //int.TryParse(userIdClaim, out int userId);
        //    //if (blockId <= 0 || passDate == default)
        //    //{
        //    //    ViewBag.Error = "Invalid block or date";

        //    //    ViewBag.Societies = await _societyService.GetAllSocietyAsync(userId);
        //    //    return View();
        //    //}
        //    //await _memberService.GeneratePass(blockId, passDate);
        //    //var QRlist = await _flatRepository.GetQR(blockId);

        //    //return View("/Views/MembersQrList/QrList.cshtml", QRlist);

        //    var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
        //    int.TryParse(userIdClaim, out int userId);

        //    // Validate input
        //    if (blockId <= 0 || passDate == default)
        //    {
        //        ViewBag.Error = "Invalid block or date.";

        //        // Repopulate dropdowns (admin vs user)
        //        IEnumerable<Society> societies;
        //        if (User.IsInRole("Admin"))
        //        {
        //            societies = await _societyService.GetAllSocietyAsync();
        //            ViewBag.IsSocietyReadonly = false;
        //        }
        //        else
        //        {
        //            societies = await _societyService.GetAllSocietyAsync(userId);
        //            ViewBag.IsSocietyReadonly = true;
        //        }

        //        ViewBag.Societies = societies;

        //        return View("/Views/MembersQrList/MembersQrList.cshtml");
        //    }

        //    // Generate QR and show result
        //    await _memberService.GeneratePass(blockId, passDate);
        //    var QRlist = await _flatRepository.GetQR(blockId);

        //    return View("/Views/MembersQrList/QrList.cshtml", QRlist);
        //}

        [HttpPost]
        public async Task<IActionResult> GeneratePass(int societyId, int blockId, DateTime passDate)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            ViewBag.SelectedSocietyId = societyId;

            // Validate inputs
            if (societyId <= 0 || blockId <= 0 || passDate == default)
            {
                ViewBag.Error = "Invalid input. Please fill all fields correctly.";

                // Repopulate societies for the dropdown
                IEnumerable<Society> societies;
                if (User.IsInRole("Admin"))
                {
                    societies = await _societyService.GetAllSocietyAsync();
                    ViewBag.IsSocietyReadonly = false;
                }
                else
                {
                    societies = await _societyService.GetAllSocietyAsync(userId);
                    ViewBag.IsSocietyReadonly = true;
                }

                ViewBag.Societies = societies;

                return View("/Views/MembersQrList/MembersQrList.cshtml");
            }

            // Generate QR codes and display result
            await _memberService.GeneratePass(blockId, passDate);
            var QRlist = await _flatRepository.GetQR(blockId);

            return View("/Views/MembersQrList/QrList.cshtml", QRlist);
        }


        [HttpGet]
        public async Task<IActionResult> GuestQrList()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            int selectedSocietyId = 0;
            if (User.IsInRole("Admin"))
            {
                // Admin sees all societies
                societies = await _societyService.GetAllSocietyAsync();
                ViewBag.IsSocietyReadonly = false;
                selectedSocietyId = 0;
            }
            else
            {
                // User sees only assigned societies
                societies = await _societyService.GetAllSocietyAsync(userId);
                ViewBag.IsSocietyReadonly = true;
                selectedSocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.Societies = societies;
            ViewBag.SelectedSocietyId = selectedSocietyId;
            return View("/Views/GuestQrList/GuestQrList.cshtml");
        }
        [HttpPost]
        //public async Task<IActionResult> GenerateGuestPass(int societyId,int blockId, DateTime passDate)
        //{
        //    if (blockId <= 0 || passDate == default)
        //    {
        //        ViewBag.Error = "Invalid block or date";
        //        var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
        //        int.TryParse(userIdClaim, out int userId);
        //        ViewBag.Societies = await _societyService.GetAllSocietyAsync(userId);
        //        return View();
        //    }
        //    await _memberService.GenerateGuestPass(blockId, passDate);
        //    var QRlist = await _flatRepository.GetGuestQR(blockId);

        //    return View("/Views/GuestQrList/GuestQr.cshtml", QRlist);
        //}

        public async Task<IActionResult> GenerateGuestPass(int societyId, int blockId, DateTime passDate)
        {
            if (blockId <= 0 || passDate == default(DateTime) /*|| passDate < DateTime.Today*/)
            {
                ViewBag.Error = "Invalid block or date";

                var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
                int.TryParse(userIdClaim, out int userId);

                // Load societies based on user role or id
                IEnumerable<Society> societies;
                if (User.IsInRole("Admin"))
                {
                    societies = await _societyService.GetAllSocietyAsync();
                    ViewBag.IsSocietyReadonly = false;
                }
                else
                {
                    societies = await _societyService.GetAllSocietyAsync(userId);
                    ViewBag.IsSocietyReadonly = true;
                }

                ViewBag.Societies = societies;
                ViewBag.SelectedSocietyId = societyId;

                // Pass back an empty model or default model as needed by your view
                return View();
            }

            await _memberService.GenerateGuestPass(blockId, passDate);
            var QRlist = await _flatRepository.GetGuestQR(blockId);

            return View("/Views/GuestQrList/GuestQr.cshtml", QRlist);
        }


        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(b => new {
                blockId = b.BlockId,
                blockName = b.BlockNumber
            });
            return Json(result);
        }
    }
}
