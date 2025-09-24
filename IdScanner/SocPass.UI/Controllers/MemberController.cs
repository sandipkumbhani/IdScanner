using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly IFlatService _flatRepository;
        public MemberController(IMemberService memberService, IBlockService blockService,ISocietyService societyService,IFlatService flatService)
        {
            _memberService = memberService;
            _blockService = blockService;
            _societyService = societyService;
            _flatRepository = flatService;
            
        }
        //public async Task<IActionResult> MemberList(int flatId)
        //{
        //    IList<Member> MemberList = await _memberService.GetAllMember(flatId);
        //    ViewBag.MemberList = MemberList;
        //    return View("~/Views/Member/MemberList.cshtml");
        //}
        [HttpGet]
        public async Task<IActionResult> AddMember()
        {
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.Societies = societies;
            var model = new MemberCreateRequest();
            return View("/Views/Member/AddMember.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] MemberCreateRequest memberCreateRequest)
        {
            if (memberCreateRequest == null)
            {
                return BadRequest("Invalid data");
            }

            if (!ModelState.IsValid)
            {
                return View("AddMember", memberCreateRequest);
            }

            await _memberService.AddMemberAsync(memberCreateRequest);

            return RedirectToAction("FlatList", "Flat");
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

        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId)
        {
            var flats = await _flatRepository.GetFlatByBlockId(blockId);
            var result = flats.Select(f => new {
                flatId = f.FlatId,
                flatNumber = f.FlatNumber     
            });
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> AddGuest()
        {
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.Societies = societies;
            var model = new MemberCreateRequest();
            return View("/Views/Guest/AddGuest.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGuest([FromBody] MemberCreateRequest memberCreateRequest)
        {
            if (memberCreateRequest == null)
            {
                return BadRequest("Invalid data");
            }

            if (!ModelState.IsValid)
            {
                return View("AddGuest", memberCreateRequest);
            }

            await _memberService.AddAndUpdateGuestAsync(memberCreateRequest);

            return RedirectToAction("FlatList", "Flat");
        }

      
[HttpGet]
    public async Task<IActionResult> DownloadTemplate(int societyId, int blockId)
    {
        var flats = await _flatRepository.GetFlatByBlockId(blockId);

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

                    var flat = (await _flatRepository.GetAllFlatAsync())
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
