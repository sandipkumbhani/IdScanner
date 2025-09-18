using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Application.Services;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using System.Net.Sockets;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet("GetGuestByid")]
        public async Task<IActionResult> GetGuestByIdAsync(int flatId)
        {
            var guest = await _memberService.GetGuestByIdAsync(flatId);
            return Ok(guest);
        }
        [HttpPut("Update-Guest")]
        public async Task<IActionResult> CreateAndUpdateGuestAsync([FromBody] MemberCreateRequest request)
        {
            try
            {
                await _memberService.CreateAndUpdateGuestAsync(
                request.FlatId,
                request.NumberOfAdults,
                request.ChildAges

            );
                return Ok("Guest Updating successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("add-update-member")]
        public async Task<IActionResult> CreateAndUpdateMemberAsync([FromBody] MemberCreateRequest request)
        {
            try
            {
                await _memberService.CreateAndUpdateMemberAsync(
                request.FlatId,
                request.NumberOfAdults,
                request.ChildAges

            );
                return Ok("Member Updating successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("GetMemberByid")]
        public async Task<IActionResult> GetMemberByIdAsync(int flatId)
        {
            var members = await _memberService.GetMemberByIdAsync(flatId);
            return Ok(members);
        }
    }
}
