using Microsoft.AspNetCore.Mvc;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        public BlockController(IBlockService blockService,ISocietyService societyService)
        {
            _blockService = blockService;
            _societyService= societyService;
        }
        public async Task<IActionResult> BlockList()
        {
            IList<Block> blockList = await _blockService.GetAllBlockAsync();
            ViewBag.blockList = blockList;
            return View("~/Views/Block/BlockList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddBlock(int? blockid)
        {
            // Always populate society list
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.SocietyList = societies;

            if (blockid == null)
            {
                return View(new Block());
            }

            var block = await _blockService.GetBlockByIdAsync(blockid.Value);
            return View(block);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlock(Block block)
        {
            var societies = await _societyService.GetAllSocietyAsync();
            ViewBag.SocietyList = societies;
            string NameMsg = string.Empty;
            if (string.IsNullOrEmpty(block.BlockNumber))
            {
                NameMsg = "Please Enter Block Number.";
                ViewBag.BlockNumberMsg = NameMsg;
            }
            string BlockNumberMsg = string.Empty;
            if (string.IsNullOrEmpty(block.BlockNumber))
            {
                BlockNumberMsg = "Please Enter Block Number.";
                ViewBag.BlockNumberMsg = BlockNumberMsg;
            }

            if (ViewBag.BlockNumberMsg != null)
            {
                return View(block);
            }

            if (block.BlockId == 0)
            {
                await _blockService.AddBlockAsync(block);
            }
            else
            {
                await _blockService.UpdateBlockAsync(block);
            }

            return RedirectToAction("BlockList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBlock(int blockid)
        {
            try
            {
                await _blockService.DeleteBlockAsync(blockid);
                return RedirectToAction("BlockList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"User with ID {blockid} not found: {ex.Message}";
                return View("Error");
            }
        }

    }
}
