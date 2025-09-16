using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Application.Services;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocietyController : Controller
    {
        private readonly ISocietyService _societyService;
        public SocietyController(ISocietyService societyService)
        {
            _societyService = societyService;

        }
        [HttpGet("GetSocietyById")]
        public async Task<IActionResult> GetSocietyById(int societyId)
        {
            try
            {
                var society = await _societyService.GetById(societyId);
                return Ok(society);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("getAllSociety")]
        public async Task<IActionResult> GetAllSociety()
        {
            var societies = await _societyService.GetAllSocietyAsync();
            return Ok(societies);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateSociety([FromBody] Society society)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newSoscity = await _societyService.CreateSocietyAsync(society);
                return Ok(newSoscity);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"someting Went Wrong");
            }
        }
        [HttpPut("Update-society/{societyId}")]
        public async Task<IActionResult> UpdateSocietyAsync(int societyId, [FromBody] Society society)
        {
            var existingSociety = await _societyService.GetById(societyId);
            if (existingSociety == null)
            {
                return NotFound($"Society with ID {societyId} not found.");
            }

            if (societyId != society.SocietyId)
            {
                return BadRequest("Society ID mismatch.");
            }
            try
            {
                var updated = await _societyService.UpdateAsync(societyId, society);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpDelete("Delete-Society")]
        public async Task<IActionResult> DeleteSocietyAsync(int societyId)
        {
            try
            {
                await _societyService.DeleteSocietyById(societyId);
                return Ok($"Society with ID {societyId} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Society with ID {societyId} not found: {ex.Message}");
            }
        }
    }
}
