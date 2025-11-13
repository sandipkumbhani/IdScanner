using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

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
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            IEnumerable<Block> blocklist;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                blocklist = await _blockService.GetAllBlockAsync();
            }
            else
            {
                var societies = await _societyService.GetSocietyByUserId(userId);
                var society = societies.FirstOrDefault();

                if (society != null)
                {
                    blocklist = (await _blockService.GetAllBlockAsync())
                                 .Where(e => e.SocietyId == society.SocietyId)
                                 .ToList();
                }
                else
                {
                    blocklist = new List<Block>();
                }
            }

            ViewBag.blockList = blocklist.ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddBlock(int? blockid)
        {
            string Title;
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;
            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                societies = await _societyService.GetSocietyByUserId(userId);
            }
            if (blockid == null)
            {
                var newBlock = new Block();
                Title = "Add";
                if (!User.IsInRole("Admin"))
                {
                    var assignedSociety = societies.FirstOrDefault();
                    if (assignedSociety != null)
                        newBlock.SocietyId = assignedSociety.SocietyId;

                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", newBlock.SocietyId);
                    ViewBag.IsSocietyReadonly = true;
                }
                else
                {
                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name");
                    ViewBag.IsSocietyReadonly = false;
                }
                ViewBag.Title = Title;
                return View(newBlock);
            }
            Title = "Edit";
            var block = await _blockService.GetBlockByIdAsync(blockid.Value);
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", block.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");
            ViewBag.Title = Title;

            return View(block);
        }
        [HttpPost]
        public async Task<IActionResult> AddBlock(Block block)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                societies = await _societyService.GetSocietyByUserId(userId);
            }
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", block.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            if (string.IsNullOrEmpty(block.BlockNumber))
            {
                ViewBag.BlockNumberMsg = "Please enter block number.";
                return View(block);
            }
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
                TempData["SuccessMessage"] = message;
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
