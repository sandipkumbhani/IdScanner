using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFlatMappingController : Controller
    {
        private readonly IUserFlatMappingService _userFlatMappingService;
        public UserFlatMappingController(IUserFlatMappingService userFlatMappingService)
        {
            _userFlatMappingService = userFlatMappingService;
        }
        [HttpGet("GetQrByUserId")]
        public async Task<IActionResult>GetQrByUserId(int userid,int eventId )
        {
            try
            {
                var result = await _userFlatMappingService.GetQrByUserId(userid,eventId);
                if (result == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
