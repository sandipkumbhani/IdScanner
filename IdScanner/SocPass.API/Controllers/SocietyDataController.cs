using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Society")]
    public class SocietyDataController : Controller
    {
        private readonly ISocietyDataService _societyDataService;
        public SocietyDataController(ISocietyDataService societyDataService)
        {
            _societyDataService = societyDataService;
        }

        [HttpGet("Get-SocietyData")]
        public async Task<IActionResult> GetSocietyData()
        {
            var societyData = await _societyDataService.GetSocietyDataAsync();
            return Ok(societyData);
        }

        [HttpPost("Add-Societydata")]
        public async Task<IActionResult> CreateSocietyDataAsync([FromBody] SocietyDataCreateRequest request)
        {
            try
            {
                await _societyDataService.CreateSocietyDataAsync(request);
                return Json(new { success = true, message = "Society Data saved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPut("Update-Societydata")]
        public async Task<IActionResult> UpdateMenuAsync([FromBody] SocietyData societyData)
        {
            try
            {
                var updatedData = await _societyDataService.UpdateSocietyDataAsync(societyData);
                return Ok(updatedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("GetBySocietyDataId")]
        public async Task<IActionResult> GetSocietyIdById(int societyDataId)
        {
            try
            {
                var Data = await _societyDataService.GetSocietyDataByIdAsync(societyDataId);
                return Ok(Data);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
            
        [HttpDelete("Delete-SocietyData")]
        public async Task<IActionResult> DeleteSocietyAsync(int societyDataId)
        {
            try
            {
                await _societyDataService.DeleteSocietyDataByIdAsync(societyDataId);
                return Ok($"Society Data with ID {societyDataId} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Society Data ID {societyDataId} not found: {ex.Message}");
            }
        }


    }
}
