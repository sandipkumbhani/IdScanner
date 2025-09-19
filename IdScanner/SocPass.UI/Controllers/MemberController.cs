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



    }
}
