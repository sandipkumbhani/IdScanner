using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.Societies = societies;
            return View("/Views/MembersQrList/MembersQrList.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> GeneratePass(int blockId, DateTime passDate)
        {
            if (blockId <= 0 || passDate == default)
            {
                ViewBag.Error = "Invalid block or date";
                ViewBag.Societies = await _societyService.GetAllSocietyAsync();
                return View();
            }
            await _memberService.GeneratePass(blockId, passDate);
            var QRlist = await _flatRepository.GetQR(blockId);

            return View("/Views/MembersQrList/QrList.cshtml", QRlist);
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
