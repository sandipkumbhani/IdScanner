using IdScanner.Application.Interface;
using IdScanner.Application.Services;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace ldScanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyMasterController : Controller
    {
        private readonly ICompanyMasterService _companyMasterService;
        public CompanyMasterController(ICompanyMasterService companyMasterService)
        {
            _companyMasterService = companyMasterService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CompanyMaster modelUsers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _companyMasterService.CreateCompanyMasterAsync(modelUsers);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"someting Went Wrong");
            }

        }
        [HttpGet("get-all-CompanyMaster")]
        public async Task<IActionResult> GetAllCompanyMaster()
        {
            var companyMasters = await _companyMasterService.GetAllCompanyMasterAsync();
            return Ok(companyMasters);
        }
        [HttpPut("Update-CompanyMaster/{companyid}")]
        public async Task<IActionResult> UpdateCompanyMasterAsync(int companyid, [FromBody] CompanyMaster companyMaster)
        {
            var existingCompany = await _companyMasterService.GetCompanyMasterById(companyid);
            if (companyid != companyMaster.CompanyId)
            {
                return BadRequest("User Company ID mismatch.");
            }
            try
            {
                var updated = await _companyMasterService.UpdateCompanyMasterAsync(companyid, companyMaster);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpDelete("Delete-CompanyMaster")]
        public async Task<IActionResult> Delete(int companyid)
        {
            try
            {
                await _companyMasterService.DeleteCompanyMasterById(companyid);
                return Ok($"companyMaster with ID {companyid} has been deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return Ok($"companyMaster with ID {companyid} not found: {ex.Message}");
            }
        }
        [HttpGet("GetCompanyMasterById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var company = await _companyMasterService.GetCompanyMasterById(id);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
