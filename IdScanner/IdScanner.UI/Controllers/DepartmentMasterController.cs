using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class DepartmentMasterController : Controller
    {
        private readonly IDepartmentMasterService _departmentMasterService;
        private readonly ICompanyMasterService _companyMasterService;
        public DepartmentMasterController(IDepartmentMasterService departmentMasterService, ICompanyMasterService companyMasterService)
        {
            _departmentMasterService = departmentMasterService
                ?? throw new ArgumentNullException(nameof(departmentMasterService));

            _companyMasterService = companyMasterService;
        }
        public async Task<IActionResult> DepartmentMasterList()
        {
            IList<Department> DepartmentMasterList = await _departmentMasterService.GetAllDepartmentMasterAsync();
            ViewBag.DepartmentMasterList = DepartmentMasterList;
            return View("~/Views/DepartmentMaster/DepartmentMasterList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddDepartmentMaster(int? id)
        {
            await InitViewBag();
            if (id == null)
            {
                return View(new Department());
            }
            var department = await _departmentMasterService.GetDepartmentByIdAsync(id.Value);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartmentMaster(Department departmentMaster)
        {
            await InitViewBag();
            string NameMsg = string.Empty;
            if (string.IsNullOrEmpty(departmentMaster.DepartmentName))
            {
                NameMsg = "Please Enter Name.";
                ViewBag.NameMsg = NameMsg;
            }

            string CityMsg = string.Empty;
            if (string.IsNullOrEmpty(departmentMaster.City))
            {
                CityMsg = "Please Enter City";
                ViewBag.CityMsg = CityMsg;
            }
            if (departmentMaster.CompanyId == 0)
            {
                ViewBag.CompanyMsg = "Please Select Company.";
            }

            if (ViewBag.NameMsg != null || ViewBag.CityMsg != null || ViewBag.CompanyMsg != null)
            {
                return View(departmentMaster);
            }
            if (departmentMaster.DepartmentId == 0)
            {
                await _departmentMasterService.AddDepartmentAsync(departmentMaster);
            }
            else
            {
                await _departmentMasterService.UpdateDepartmentAsync(departmentMaster);
            }
            return RedirectToAction("DepartmentMasterList");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteDepartmentMaster(int id)
        {
            try
            {
                await _departmentMasterService.DeleteDepartmentAsync(id);
                return RedirectToAction("DepartmentMasterList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Department with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }

        private async Task InitViewBag()
        {
            IList<CompanyMaster> company = await _companyMasterService.GetAllCompanyMasterAsync();
            ViewBag.CompanyList = company;
        }

    }
}
