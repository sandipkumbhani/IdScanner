using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin", "Society")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatService;
        private readonly GlobalClass _globalClass;
        public MemberController(IMemberService memberService, IBlockService blockService, ISocietyService societyService, IFlatService flatService, GlobalClass globalClass)
        {
            _memberService = memberService;
            _blockService = blockService;
            _societyService = societyService;
            _flatService = flatService;
            _globalClass = globalClass;
        }
        [HttpGet]
        public async Task<IActionResult> AddMember()
        {
            bool isAdmin = User.IsInRole("Admin");
            var societies = await _societyService.GetSocietyAsync();
            var model = new MemberCreateRequest();
            if (!isAdmin)
            {
                model.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }
            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] MemberCreateRequest model)
        {
            await _memberService.AddMemberAsync(model);
            return Json(new
            {
                success = true,
                message = "Member added successfully!",
            });
        }
        [HttpGet]
        public async Task<IActionResult> AddGuest()
        {
            bool isAdmin = User.IsInRole("Admin");
            var societies = await _societyService.GetSocietyAsync();

            var guest = new MemberCreateRequest();
            if (!isAdmin)
            {
                guest.SocietyId = societies.FirstOrDefault()?.SocietyId ?? 0;
            }

            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.Societies = societies;
            return View("/Views/Guest/AddGuest.cshtml", guest);
        }

        [HttpPost]
        public async Task<IActionResult> AddGuest([FromBody] MemberCreateRequest memberCreateRequest)
        {
            var result = await _memberService.AddAndUpdateGuestAsync(memberCreateRequest);
            return RedirectToAction("FlatList", "Flat");
        }
        [HttpGet]
        public async Task<IActionResult> DownloadTemplate(int societyId, int blockId)
        {
            var flats = await _flatService.GetFlatByBlockId(blockId);

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Flats");

                // headers
                ws.Cell(1, 1).Value = "FlatNumber";
                ws.Cell(1, 2).Value = "NumberOfAdults";
                ws.Cell(1, 3).Value = "NumberOfChildren";
                ws.Cell(1, 4).Value = "ChildrenAges (comma separated)";
                int row = 2;
                foreach (var flat in flats)
                {
                    ws.Cell(row, 1).Value = flat.FlatNumber;
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Society_{societyId}_Block_{blockId}_Template.xlsx");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadMembers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using (var workbook = new XLWorkbook(file.OpenReadStream()))
            {
                var ws = workbook.Worksheet(1); // first sheet
                var rows = ws.RangeUsed().RowsUsed().Skip(1); // skip headers

                foreach (var row in rows)
                {
                    string flatNumber = row.Cell(1).GetString();
                    int adults = row.Cell(2).GetValue<int>();
                    int children = row.Cell(3).GetValue<int>();
                    string childrenAgesStr = row.Cell(4).GetString();

                    var childAges = childrenAgesStr
                        .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x.Trim()))
                        .ToList();

                    var flat = (await _flatService.GetFlatAsync())
                               .FirstOrDefault(f => f.FlatNumber == flatNumber);

                    if (flat != null)
                    {
                        var request = new MemberCreateRequest
                        {
                            FlatId = flat.FlatId,
                            NumberOfAdults = adults,
                            ChildAges = childAges
                        };

                        await _memberService.AddMemberAsync(request);
                    }
                }
            }
            return RedirectToAction("FlatList", "Flat");
        }


    }
}
