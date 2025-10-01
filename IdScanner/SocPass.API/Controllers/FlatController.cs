using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : Controller
    {
        private readonly IFlatService _flatService;
        public FlatController(IFlatService flatService)
        {
            _flatService = flatService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Flat flat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newFlat = await _flatService.CreateFlatAsync(flat);
                return Ok(newFlat);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"someting Went Wrong");
            }
        }
        [HttpGet("GetFlatById")]
        public async Task<IActionResult> GetById(int FlatId)
        {
            try
            {
                var flat = await _flatService.GetByIdAsync(FlatId);
                return Ok(flat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("getAllFlat")]
        public async Task<IActionResult> GetAllFlat()
        {
            var flats = await _flatService.GetAllFlatAsync();
            return Ok(flats);
        }
        [HttpPut("Update-flat")]
        public async Task<IActionResult> UpdateFlatAsync([FromBody] Flat flat)
        {
            if (flat == null)
            {
                return NotFound("Id Not found");
            }
            try
            {
                var updated = await _flatService.UpdateFlatAsync(flat);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("GetFlatByBlockid")]
        public async Task<IActionResult> GetFlatByBlockid(int blockid)
        {
            if (blockid <= 0)
            {
                return BadRequest("Invalid BlockId ID.");
            }
            var flats = await _flatService.GetFlatByBlockID(blockid);
            if (flats == null)
            {
                return NotFound("No BlockId found for this company.");
            }
            return Ok(flats);
        }
        [HttpGet("GetQR")]
        public async Task<IActionResult> GetQR(int blockid)
        {
            try
            {
                var result = await _flatService.GetMemberQr(blockid);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetGuestQR")]
        public async Task<IActionResult> GetGuestQR(int blockid)
        {
            try
            {
                var result = await _flatService.GetGuestQr(blockid);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
