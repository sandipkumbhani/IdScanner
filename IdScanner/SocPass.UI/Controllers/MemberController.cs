using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

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

        public async Task<IActionResult> MemberList(int flatId)
        {
            IList<Member> MemberList = await _memberService.GetAllMember(flatId);
            ViewBag.MemberList = MemberList;
            return View("~/Views/Member/MemberList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddMember()
        {

            return View("/Views/Member/AddMember.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(MemberCreateRequest memberCreateRequest)
        {
            if (memberCreateRequest == null)
            {
                return View(new MemberCreateRequest());
            }
            await _memberService.AddMemberAsync(memberCreateRequest);
            return RedirectToAction("MenuMasterList");
        }
		[HttpGet]
        public async Task<IActionResult> getSociety()
        {
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.Societies = societies;
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetFlatsByBlock(int blockId)
        {
            var flats = await _flatRepository.GetFlatByBlockId(blockId); 
            return Json(flats);
        }
       


    }
}
