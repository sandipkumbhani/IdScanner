using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocPass.Application.Interface;
using SocPass.Domain.Model;

namespace SocPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> AddSubscriptionAsync([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addSubscription = await _subscriptionService.addsubscriptionAsync(subscription);
                return Ok(addSubscription );
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"someting Went Wrong");
            }
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int subscriptionId)
        {
            try
            {
                var result = await _subscriptionService.GetById(subscriptionId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetBySocietyId")]
        public async Task<IActionResult> GetBySocietyId(int societyId)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionBySocietyIdAsync(societyId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Update-Subscription/{subscriptionId}")]
        public async Task<IActionResult> UpdateMenuAsync(int subscriptionId, [FromBody] Subscription subscription)
        {
            var subscriptionUpdate = await _subscriptionService.GetById(subscriptionId);
            if (subscriptionUpdate == null && subscriptionId != subscription.SubscriptionId)
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
                    var updatedUser = await _subscriptionService.UpdateSubscriptionAsync(subscriptionId, subscription);
                    return Ok(updatedUser);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
        [HttpDelete("Delete-Subscription")]
        public async Task<IActionResult> DeleteAsync(int subscriptionId)
        {
            try
            {
                await _subscriptionService.DeleteSubscriptionAsync(subscriptionId);
                return Ok($"Subscription with ID {subscriptionId} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"Menu with ID {subscriptionId} not found: {ex.Message}");
            }
        }
        [HttpGet("Get-All-Subscription")]
        public async Task<IActionResult> GetAllSubscription()
        {
            var result = await _subscriptionService.GetAllSubscription();
            return Ok(result);
        }

        [HttpGet("GetExistsSocietyData")]
        public async Task<IActionResult> GetExistsSocietyData(int societyId, int subscriptionId = 0)
        {
            if (societyId <= 0)
                return BadRequest(false);

            try
            {
                bool exists = await _subscriptionService.ExistsSocietyDataAsync(societyId, subscriptionId);
                return Ok(exists); // returns true if society already exists, false otherwise
            }
            catch
            {
                return StatusCode(500, false);
            }
        }

    }
}
