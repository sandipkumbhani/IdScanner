using IdScanner.Application.Interface;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuMasterController : Controller
    {
        private readonly IMenuMasterService _menuMasterService;

        public MenuMasterController(IMenuMasterService MenuMasterService)
        {
            _menuMasterService = MenuMasterService;
        }
        [HttpGet("Get-All-Menu-Master")]
        public async Task<IActionResult> GetAllMenuMaster()
        {

            var users = await _menuMasterService.GetModelMenuMastersAsync();
            return Ok(users);
        }
        [HttpPost("Menu-Master")]
        public async Task<IActionResult> CreateMenuMaster([FromBody] MenuMaster modelMenuMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuMaster = await _menuMasterService.CreateMenuMasterAsync(modelMenuMaster);
            return Ok(menuMaster);
        }
        [HttpDelete("Delete-Menu-Master")]
        public async Task<IActionResult> DeleteMenuAsync(int id)
        {
            try
            {
                await _menuMasterService.DeleteMenuById(id);
                return Ok($"Menu with ID {id} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Menu with ID {id} not found: {ex.Message}");
            }
        }
        [HttpPut("Update-Menu/{menuid}")]
        public async Task<IActionResult> UpdateMenuAsync(int menuid, [FromBody] MenuMaster menuMaster)
        {
            var existingUser = await _menuMasterService.GetMenuMsaterById(menuid);
            if (existingUser == null && menuid != menuMaster.MenuId)
            {
                return BadRequest("Menu ID mismatch.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var updatedUser = await _menuMasterService.UpdateMenuAsync(menuid, menuMaster);
                    return Ok(updatedUser);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
        [HttpGet("GetByMenuId")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var menumaster = await _menuMasterService.GetMenuMsaterById(id);
                return Ok(menumaster);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
        //[HttpGet("GetMenusByUserId")]
        //public async Task<IActionResult> GetMenusByUserId(int userId)
        //{
        //    try
        //    {
        //        var menumaster = await _menuMasterService.GetMenusByUserIdAsync(userId);
        //        return Ok(menumaster);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }

        //}
    }
}
