using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : Controller
    {
        private readonly IBlockService _blockService;
        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpGet("GetAllBlock")]
        public async Task<IActionResult> GetAllBlock()
        {
            var blocks = await _blockService.GetAllBlockAsync();
            return Ok(blocks);
        }

        [HttpPost("Create-Block")]
        public async Task<IActionResult> CreateBlock([FromBody] Block block)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newBlock = await _blockService.CreateBlockAsync(block);
                return Ok(newBlock);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Something Went Wrong");
            }
        }

        [HttpGet("GetBlockById")]
        public async Task<IActionResult> GetBlockById(int blockid)
        {
            try
            {
                var block = await _blockService.GetBlockByIdAsync(blockid);
                return Ok(block);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Update-Block/{blockid}")]
        public async Task<IActionResult> UpdateBlockAsync(int blockid, [FromBody] Block block)
        {
            var existingBlock = await _blockService.GetBlockByIdAsync(blockid);
            if (existingBlock == null)
            {
                return NotFound($"Block with ID {blockid} not found.");
            }

            try
            {
                var UpdatedBlock = await _blockService.UpdateBlockAsync(blockid, block);
                return Ok(UpdatedBlock);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("Delete-Block")]
        public async Task<IActionResult> DeleteBlockAsync(int blockid)
        {
            var existingBlock = await _blockService.GetBlockByIdAsync(blockid);
            if (existingBlock == null)
            {
                return NotFound($"Block with ID {blockid} not found.");
            }
            await _blockService.DeleteBlockByIdAsync(blockid);
            return Ok($"Block with ID {blockid} deleted successfully.");
        }

    }
}
