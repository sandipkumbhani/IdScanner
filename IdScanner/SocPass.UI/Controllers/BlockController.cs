using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken("Admin","Society")]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly GlobalClass _globalClass;
        public BlockController(IBlockService blockService, ISocietyService societyService, GlobalClass globalClass)
        {
            _blockService = blockService;
            _societyService = societyService;
            _globalClass = globalClass;
        }
        [HttpGet]
        public async Task<IActionResult> BlockList()
        {
           var blocklist = await _blockService.GetBlockAsync();
            ViewBag.blockList = blocklist;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddBlock(int? blockid)
        {
            bool isAdmin = User.IsInRole("Admin");
            var societies = await _societyService.GetSocietyAsync();
            Block model;
            string title;

            if (blockid == null)
            {
                title = "Add";
                model = new Block();
                if (!isAdmin)
                {
                    var assignedSociety = societies.FirstOrDefault();
                    if (assignedSociety != null)
                        model.SocietyId = assignedSociety.SocietyId;
                }
            }
            else
            {
                title = "Edit";
                model = await _blockService.GetBlockByIdAsync(blockid.Value);
            }
            ViewBag.Title = title;
            ViewBag.IsSocietyReadonly = !isAdmin;
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", model.SocietyId);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddBlock(Block block)
        {
            try
            {
                string message;

                if (block.BlockId == 0)
                {
                    message = await _blockService.AddBlockAsync(block);
                }
                else
                {
                    message = await _blockService.UpdateBlockAsync(block);
                }
                if (message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.ErrorMessage = message;
                    return View(block);
                }
                return RedirectToAction("BlockList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(block);
            }
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

        [HttpGet]
        public async Task<JsonResult> GetBlocksBySociety(int societyId)
        {
            var blocks = await _blockService.GetBlockBySocietyId(societyId);
            var result = blocks.Select(b => new
            {
                blockId = b.BlockId,
                blockName = b.BlockNumber
            });
            return Json(result);
        }
    }
}
