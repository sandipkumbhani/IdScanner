using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class ReportController : Controller
    {
        private readonly ISocietyService _societyService;
        private readonly IBlockService _blockService;
        private readonly IFlatService _flatService;
        private readonly IMemberService _memberService;

        public ReportController(ISocietyService societyService, IBlockService blockService, IFlatService flatService,IMemberService memberService)
        {
            _societyService = societyService;
            _blockService = blockService;
            _flatService = flatService;
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> MemberReport()
        {
            //var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            //int.TryParse(userIdClaim, out int userId);
            //ViewBag.Societies = await _societyService.GetAllSocietyAsync(userId);

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

            return View("/Views/Report/ReportDataList.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            return Json(blocks.Select(b => new { blockId = b.BlockId, blockName = b.BlockNumber }));
        }

        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId, string? selectedDate)
        {
            var flats = await _flatService.GetFlatByBlockId(blockId);
            var flatReports = new List<object>();

            DateOnly? filterDate = null;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                DateOnly.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate);
                filterDate = parsedDate;
            }

            foreach (var f in flats)
            {
                var members = await _memberService.GetAllMember(f.FlatId);

                var filteredMembers = filterDate.HasValue
                    ? members.Where(m => m.PassDate == filterDate.Value).ToList()
                    : members;

                int visitedAdults = filteredMembers.Count(m => m.Visited && !m.IsChild);
                int visitedChildren = filteredMembers.Count(m => m.Visited && m.IsChild);
         
                int pending = f.TotalMember - (visitedAdults + visitedChildren);

                

                flatReports.Add(new
                {
                    f.FlatId,
                    f.FlatNumber,
                    f.NumberOfAdult,
                    f.NumberOfChild,
                    f.TotalMember,
                    VisitedAdults = visitedAdults,
                    VisitedChildren = visitedChildren,
                  
                    Pending = pending,
                    HasMembersForDate = filteredMembers.Any()
                });
            }

            if (filterDate.HasValue && !flatReports.Any(f => (bool)f.GetType().GetProperty("HasMembersForDate")!.GetValue(f)!))
            {
                return Json(new object[0]);
            }

            return Json(flatReports);
        }

    }
}
