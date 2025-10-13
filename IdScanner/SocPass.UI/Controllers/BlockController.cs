using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;

namespace SocPass.UI.Controllers
{
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        public BlockController(IBlockService blockService, ISocietyService societyService)
        {
            _blockService = blockService;
            _societyService = societyService;
        }
        
        [HttpGet]
        public async Task<IActionResult> BlockList()
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            // Get all blocks (no filtering needed here)
            var blockList = (await _blockService.GetAllBlockAsync()).ToList();

            ViewBag.blockList = blockList;

            // Pass admin flag
            ViewBag.IsAdmin = User.IsInRole("Admin");

            // Pass user's society name if not admin
            if (!User.IsInRole("Admin"))
            {
                var societies = await _societyService.GetAllSocietyAsync(userId);
                var userSocietyName = societies.FirstOrDefault()?.Name ?? "";
                ViewBag.UserSocietyName = userSocietyName;
            }
            else
            {
                ViewBag.UserSocietyName = ""; // optional
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddBlock(int? blockid)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            // Admin = access all societies, else user-specific
            if (User.IsInRole("Admin"))
                societies = await _societyService.GetAllSocietyAsync();
            else
                societies = await _societyService.GetAllSocietyAsync(userId);

            // ---------- NEW BLOCK ----------
            if (blockid == null)
            {
                var newBlock = new Block();

                if (!User.IsInRole("Admin"))
                {
                    // For non-admins: preselect and lock the dropdown
                    var assignedSociety = societies.FirstOrDefault();
                    if (assignedSociety != null)
                        newBlock.SocietyId = assignedSociety.SocietyId;

                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", newBlock.SocietyId);
                    ViewBag.IsSocietyReadonly = true;
                }
                else
                {
                    // Admins: all societies visible and editable
                    ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name");
                    ViewBag.IsSocietyReadonly = false;
                }

                return View(newBlock);
            }

            // ---------- EDIT BLOCK ----------
            var block = await _blockService.GetBlockByIdAsync(blockid.Value);
            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", block.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            return View(block);
        }


        [HttpPost]
        public async Task<IActionResult> AddBlock(Block block)
        {
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
                societies = await _societyService.GetAllSocietyAsync();
            else
                societies = await _societyService.GetAllSocietyAsync(userId);

            ViewBag.SocietyList = new SelectList(societies, "SocietyId", "Name", block.SocietyId);
            ViewBag.IsSocietyReadonly = !User.IsInRole("Admin");

            // --- Validation ---
            if (string.IsNullOrEmpty(block.BlockNumber))
            {
                ViewBag.BlockNumberMsg = "Please Enter Block Number.";
                return View(block);
            }

            // --- Save/Update ---
            if (block.BlockId == 0)
                await _blockService.AddBlockAsync(block);
            else
                await _blockService.UpdateBlockAsync(block);

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
