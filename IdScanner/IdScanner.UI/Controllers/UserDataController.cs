using System.ComponentModel.Design;
using System.Reflection;
using Google.Apis.Drive.v3.Data;
using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using QRCoder;
using ZXing.QrCode.Internal;

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
        public async Task<IActionResult> AddUserData(int? id)
        {
            await InitViewBag();
            if (id == null)
            {
                var model = new UserData();
                return View(model);
            }
            var user = await _userDataService.GetById(id.Value);
            return View("~/Views/UserData/AddUserData.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserData(UserData userData, IFormFile ImageFile, IFormFile policeFile, IFormFile medicalFile, IFormFile signatureFile)
        {
            await InitViewBag();

            if (ImageFile == null || ImageFile.Length == 0)
            {
                ViewBag.ImageFileRequiredMsg = "Profile Image is required.";
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {

                var fileName = Path.GetFileName(ImageFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                userData.PhotoUrl = filePath;
            }

            if (policeFile == null || policeFile.Length == 0)
            {
                ViewBag.policeFileRequiredMsg = "Police File is required.";
            }

            if (policeFile != null && policeFile.Length > 0)
            {
                var fileName = Path.GetFileName(policeFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await policeFile.CopyToAsync(stream);
                }
                userData.PoliceVerificationCertificateUrl = filePath;
            }

            if (medicalFile == null || medicalFile.Length == 0)
            {
                ViewBag.medicalFileFileRequiredMsg = "Medical File is required.";
            }

            if (medicalFile != null && medicalFile.Length > 0)
            {
                var fileName = Path.GetFileName(medicalFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await medicalFile.CopyToAsync(stream);
                }
                userData.MedicalCertificateUrl = filePath;
            }
			if (signatureFile == null || signatureFile.Length == 0)
			{
				ViewBag.signatureFileFileRequiredMsg = "Signature File is required.";
			}

			if (signatureFile != null && signatureFile.Length > 0)
			{

				var fileName = Path.GetFileName(signatureFile.FileName);
				var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				var filePath = Path.Combine(folderPath, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await signatureFile.CopyToAsync(stream);
				}

				userData.SignatureUrl = filePath;
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
                MobileNumberMsg = "Please Enter Mobile No.";
                ViewBag.MobileNumberMsg = MobileNumberMsg;
            }

            string DesignationMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.Designation))
            {
                DesignationMsg = "Please Enter Designation.";
                ViewBag.DesignationMsg = DesignationMsg;
            }

            string StallPfNumberMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.StallPfNumber))
            {
                StallPfNumberMsg = "Please Enter StallPfNumber.";
                ViewBag.StallPfNumberMsg = StallPfNumberMsg;
            }

            string WorkSlotMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.WorkSlot))
            {
                WorkSlotMsg = "Please Enter WorkSlot.";
                ViewBag.WorkSlotMsg = WorkSlotMsg;
            }

            string AadharNumberMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.AadhaarCardNumber))
            {
                AadharNumberMsg = "Please Enter Adhar No.";
                ViewBag.AadharNumberMsg = AadharNumberMsg;
            }

            string LicenseeMsg = string.Empty;
            if (string.IsNullOrEmpty(userData.Licensee))
            {
                LicenseeMsg = "Please Enter Licensee.";
                ViewBag.LicenseeMsg = LicenseeMsg;
            }

            if (userData.IdValidTill == null || userData.IdValidTill == DateTime.MinValue)
            {
                ViewBag.IdValidTillMsg = "Please select a valid date.";
            }
            if (string.IsNullOrEmpty(userData.BloodGroup))
            {
                ViewBag.BloodGroupMsg = "Please select a valid Blood Group.";
            }
            if (userData.CompanyId == 0)
            {
                ViewBag.CompanyMsg = "Please Select Company.";
            }

            if (userData.DepartmentId == 0)
            {
                ViewBag.DepartmentMsg = "Please Select Department.";
            }

            if (ViewBag.NameMsg != null || ViewBag.MobileNumberMsg != null || ViewBag.DesignationMsg != null || ViewBag.StallPfNumberMsg != null || ViewBag.WorkSlotMsg != null || ViewBag.LicenseeMsg != null || ViewBag.IdValidTillMsg != null || ViewBag.CompanyMsg != null || ViewBag.DepartmentMsg != null || ViewBag.ImageFileRequiredMsg != null || ViewBag.policeFileRequiredMsg != null || ViewBag.medicalFileFileRequiredMsg != null || ViewBag.signatureFileFileRequiredMsg != null || ViewBag.BloodGroupMsg != null || ViewBag.AadharNumberMsg != null)
            {
                return View(userData);
            }
            if (userData.UserDataId == 0)
            {
                userData= await _userDataService.AddUserDataAsync(userData);
                await updatedQrAsync(userData.UserDataId);
			}
            else
            {
                await _userDataService.UpdateUserAsync(userData);
            }
            return RedirectToAction("UserDataList");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userData = await _userDataService.GetById(id);
            if (userData == null)
            {
                return NotFound("User not found");
            }
            //var photoUrl = userData.PhotoUrl.Replace("uc?", "thumbnail?");
            //ViewBag.photoUrl = photoUrl + "&sz=s220";
            if (!string.IsNullOrEmpty(userData.PhotoUrl))
            {
                //user.PhotoUrl = user.PhotoUrl.Replace("uc?", "thumbnail?") + "&sz=s220";
                userData.PhotoUrl = ExtractFileId(userData.PhotoUrl);
            }
            return View("~/Views/UserData/UserDetails.cshtml", userData); 
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
        private async Task InitViewBag()
        {
            IList<CompanyMaster> company = await _companyMasterService.GetAllCompanyMasterAsync();
            ViewBag.CompanyList = company;
        }
        private async Task updatedQrAsync(long userid)
        {
			string qrUrl = $"http://localhost:5201/UserData/Details/{userid}";

			using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
			using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q))
			using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
			{
				byte[] qrBytes = qrCode.GetGraphic(10);

				var qrFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRCodes");
				if (!Directory.Exists(qrFolder))
				{
					Directory.CreateDirectory(qrFolder);
				}
				var qrFileName = $"{userid}_qr.png";
				var qrPath = Path.Combine(qrFolder, qrFileName);
				await System.IO.File.WriteAllBytesAsync(qrPath, qrBytes);
				await _userDataService.UpdateQrCodeAsync(userid, "/QRCodes/" + qrFileName);
			}
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
        public async Task<IActionResult> UserIdCardList()
        {
            IList<UserData> UserDataList = await _userDataService.GetAllUserDetailsAsync();

            foreach (var user in UserDataList)
            {
                if (!string.IsNullOrEmpty(user.PhotoUrl))
                {
                    user.PhotoUrl = ExtractFileId(user.PhotoUrl);
                }
                if (!string.IsNullOrEmpty(user.SignatureUrl))
                {
                    user.SignatureUrl = ExtractFileId(user.SignatureUrl);
                }
            }

            ViewBag.UserDataList = UserDataList;
            return View("~/Views/UserData/ViewUserIdCard.cshtml");
        }

        private string ExtractFileId(string url)
        {
            if (url.Contains("id="))
            { 
                var index = url.IndexOf("id=") + 3;
                return url.Substring(index);
            }
            else if (url.Contains("/d/"))
            {
                var start = url.IndexOf("/d/") + 3 + 1;
                var end = url.IndexOf("/view");
                return url.Substring(start, end - start);
            }
            return url;
        }

    }
}
