using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;
using SocPass.UI.Filters;
using System.Security.Claims;

namespace SocPass.UI.Controllers
{
    [AuthorizeToken]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        private readonly ISocietyService _societyService;
        private readonly GlobalClass _globalClass;
        public BlockController(IBlockService blockService, ISocietyService societyService,GlobalClass globalClass)
        {
            _blockService = blockService;
            _societyService = societyService;
            _globalClass = globalClass;
        }
        [HttpGet]
        public async Task<IActionResult> BlockList()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);
            var blockList = (await _blockService.GetAllBlockAsync()).ToList();
            ViewBag.blockList = blockList;
            ViewBag.IsAdmin = User.IsInRole("Admin");
            if (!User.IsInRole("Admin"))
            {
                var societies = await _societyService.GetAllSocietyAsync(userId);
                var userSocietyName = societies.FirstOrDefault()?.Name ?? "";
                ViewBag.UserSocietyName = userSocietyName;
            }
            else
            {
                ViewBag.UserSocietyName = "";
            }
            return View();
        }
         [HttpGet]
        public async Task<IActionResult> AddBlock(int? blockid)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            var userIdClaim = HttpContext.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            IEnumerable<Society> societies;

            if (User.IsInRole("Admin"))
            {
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
            }
            if (blockid == null)
            {
                var newBlock = new Block();

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

                return View(newBlock);
            }

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
            {
                societies = await _societyService.GetAllSocietyAsync();
            }
            else
            {
                societies = await _societyService.GetAllSocietyAsync(userId);
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
             var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("AccessDenied", "AccessDenied");
            }
            if (string.IsNullOrEmpty(_globalClass.Token))
            {
                return RedirectToAction("Login", "Login");
            }
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
