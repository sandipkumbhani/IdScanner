using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class CompanyMasterController : Controller
    {
        private readonly ICompanyMasterService _companyMasterService;
        public CompanyMasterController(ICompanyMasterService companyMasterService)
        {
            _companyMasterService = companyMasterService
                ?? throw new ArgumentNullException(nameof(companyMasterService));
        }

        public async Task<IActionResult> CompanyMasterList()
        {
            IList<CompanyMaster> CompanyMasterList = await _companyMasterService.GetAllCompanyMasterAsync();
            ViewBag.CompanyMasterList = CompanyMasterList;
            return View("~/Views/CompanyMaster/CompanyMasterList.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> AddCompanyMaster(int? id)
        {
            if (id == null)
            {
                return View(new CompanyMaster());
            }
            var company = await _companyMasterService.GetCompanyByIdAsync(id.Value);
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyMaster(CompanyMaster companyMaster, IFormFile ImageFile)
        {
            string NameMsg = string.Empty;
            if(string.IsNullOrEmpty(companyMaster.CompanyName))
            {
                NameMsg = "Please Enter Name.";
                ViewBag.NameMsg = NameMsg;
            }

            string CityMsg = string.Empty;
            if(string.IsNullOrEmpty(companyMaster.City))
            {
                CityMsg = "Please Enter City";
                ViewBag.CityMsg = CityMsg;
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CompanyLogoImages");
                var filePath = Path.Combine(folderPath, fileName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                companyMaster.logo = "/CompanyLogoImages/" + fileName;
            }
            string LogoMsg = string.Empty;
            if (string.IsNullOrEmpty(companyMaster.logo))
            {
                LogoMsg = "Please Upload Company Logo.";
                ViewBag.LogoMsg = LogoMsg;
            }

            if (ViewBag.NameMsg != null ||ViewBag.CityMsg != null || ViewBag.LogoMsg != null)
            {
                return View(companyMaster);
            }

            if (companyMaster.CompanyId == 0)
            {
                await _companyMasterService.AddCompanyAsync(companyMaster);
            }
            else
            {
                await _companyMasterService.UpdateCompanyAsync(companyMaster);
            }
            return RedirectToAction("CompanyMasterList");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteCompanyMaster(int id)
        {
            try
            {
                await _companyMasterService.DeleteCompanyAsync(id);
                return RedirectToAction("CompanyMasterList");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = $"Company with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }
    }
}
