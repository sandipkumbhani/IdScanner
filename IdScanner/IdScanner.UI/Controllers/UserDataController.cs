using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel.Design;
using System.Reflection;

namespace IdScanner.UI.Controllers
{
    public class UserDataController : Controller
    {
        private readonly IUserDataService _userDataService;
        private readonly ICompanyMasterService _companyMasterService;
        private readonly IDepartmentMasterService _departmentMasterService;
        public UserDataController(IUserDataService userDataService, ICompanyMasterService companyMasterService,IDepartmentMasterService departmentMasterService)
        {
            _userDataService = userDataService;
            _companyMasterService = companyMasterService;
            _departmentMasterService = departmentMasterService;
        }
        public async Task<IActionResult> UserDataList()
        {
            IList<UserData> UserDataList = await _userDataService.GetAllUserDetailsAsync();
            ViewBag.UserDataList = UserDataList;
            return View("~/Views/UserData/UserDataList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddUserData(int? userid)
        {
            await InitViewBag(CompanyId);
            if (userid == null)
            {
                var model = new UserData();
               
                return View(model);
            }
            var user = await _userDataService.GetById(id.Value);
            return View("~/Views/UserData/AddUserData.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserData(UserData userData, IFormFile ImageFile, IFormFile policeFile, IFormFile medicalFile)
        {
            await InitViewBag();
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                userData.PhotoUrl = filePath;
            }
            if (policeFile != null && policeFile.Length > 0)
            {
                var fileName = Path.GetFileName(policeFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await policeFile.CopyToAsync(stream);
                }
                userData.PoliceVerificationCertificateUrl = filePath;
            }
            if (medicalFile != null && medicalFile.Length > 0)
            {
                var fileName = Path.GetFileName(medicalFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await medicalFile.CopyToAsync(stream);
                }
                userData.MedicalCertificateUrl = filePath;
            }

            string NameMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.Name))
            {
                NameMsg = "Please Enter Name.";
                ViewBag.NameMsg = NameMsg;
            }
            string MobileNumberMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.MobileNumber))
            {
                MobileNumberMsg = "Please Enter.";
                ViewBag.DescriptionMsg = MobileNumberMsg;
            }
            if (ViewBag.NameMsg != null || ViewBag.DescriptionMsg != null)
            {
                return View(userData);
            }
            if (userData.UserDataId == 0)
            {
                await _userDataService.AddUserDataAsync(userData);
            }
            else
            {
                await _userDataService.UpdateUserAsync(userData);
            }
            return RedirectToAction("UserDataList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUserData(int id)
        {
            try
            {
                await _userDataService.DeleteUserAsync(id);
                return RedirectToAction("UserDataList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"User with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }
        private async Task InitViewBag(int? CompanyId)
            //else
            //{
            //    await _userDataService.UpdateMenuAsync(userData);
            //}
            return RedirectToAction("UserDataList");
        }
        [HttpGet]
        public async Task<JsonResult> GetDepartmentsByCompany(int companyId)
        {
            var departments = await _departmentMasterService.GetDepartmentByCompanyId(companyId);
            var result = departments.Select(d => new
            {
                departmentId = d.DepartmentId,
                departmentName = d.DepartmentName
            });
            return Json(result);
        }
        private async Task InitViewBag()
        {
            IList<CompanyMaster> company = await _companyMasterService.GetAllCompanyMasterAsync();
            ViewBag.CompanyList = company;
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartmentsByCompany(int companyId)
        {
            var departments = await _departmentMasterService.GetDepartmentByCompanyId(companyId);
            var result = departments.Select(d => new
            {
                departmentId = d.DepartmentId,
                departmentName = d.DepartmentName
            });

            return Json(result);
        }
    }
}
