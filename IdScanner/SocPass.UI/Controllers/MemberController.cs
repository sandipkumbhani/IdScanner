using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> MemberList(int flatId)
        {
            IList<Member> MemberList = await _memberService.GetAllMember(flatId);
            ViewBag.MemberList = MemberList;
            return View("~/Views/Member/MemberList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddMemberMAster()
        {
            return View("/Views/Member/AddMember.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddMemberMAster(MemberCreateRequest memberCreateRequest)
        {
            if (memberCreateRequest == null)
            {
                return View(new MemberCreateRequest());
            }
            await _memberService.AddMemberAsync(memberCreateRequest);
            return RedirectToAction("MenuMasterList");
        }
    }
}
