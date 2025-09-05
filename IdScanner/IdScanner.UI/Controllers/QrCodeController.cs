using IdScanner.UI.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly IQrCodeService _qrCodeService;
        private readonly IWebHostEnvironment _env;

        public QrCodeController(IQrCodeService qrCodeService, IWebHostEnvironment env)
        {
            _qrCodeService = qrCodeService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateAndSave(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                ModelState.AddModelError("", "Please enter text.");
                return View("Index");
            }
            string folderPath = Path.Combine(_env.WebRootPath, "QrCodes");

            string fileName = _qrCodeService.SaveQrCode(text, folderPath);

            string fileUrl = $"/QrCodes/{fileName}";

            ViewBag.QrImageUrl = fileUrl;

            return View("Index");
        }
        [HttpPost]
        public IActionResult Scan(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.ScanResult = "Please upload a file!";
                return View("Index");
            }

            string folderPath = Path.Combine(_env.WebRootPath, "Uploads");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            string result = _qrCodeService.ReadQrCode(filePath);
            ViewBag.ScanResult = result;

            return View("Index");
        }
    }
}


