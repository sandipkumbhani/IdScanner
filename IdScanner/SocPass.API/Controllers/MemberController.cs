using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.DTO;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Society")]
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
        [HttpPut("AddPassdate")]
        public async Task<IActionResult> AddPassDate(int blockId, int EventId)
        {
            var result = await _memberService.AddMemberPassDateAsync(blockId, EventId);
            return Ok(result);
        }
        [HttpPut("AddGuestPassdate")]
        public async Task<IActionResult> AddGuestPassDate(int blockId, DateTime passDate)
        {
            var result = await _memberService.AddGuestPassDateAsync(blockId, passDate);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetMemberByMemberId")]
        public async Task<IActionResult> GetmemberById(int memberId,int EventId)
        {
            var result = await _memberService.GetMemberByMemberId(memberId, EventId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("IsVisited")]
        public async Task<IActionResult> IsVisitedAsync(int memberId, int loggedInUserId)
        {
            if (memberId <= 0)
            {
                return BadRequest(new { Message = "Valid Member Id is required." });
            }
            
            bool isVisited = await _memberService.IsVisitedAsync(memberId, loggedInUserId);
            if (isVisited)
            {
                return Ok(new { Message = "Member is visiting." });
            }
            else
            {
                return Ok(new { Message = "Member is already visiting." });
            }
        }


    }
}
