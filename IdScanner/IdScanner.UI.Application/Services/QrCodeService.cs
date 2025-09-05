using IdScanner.UI.Application.Interface;
using QRCoder;
using SkiaSharp;
using System;
using ZXing;
using ZXing.SkiaSharp;

namespace IdScanner.UI.Application.Services
{
    public class QrCodeService : IQrCodeService
    {
        public string SaveQrCode(string text, string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

                var qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrBytes = qrCode.GetGraphic(20);

                string fileName = $"qr_{Guid.NewGuid()}.png";
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, qrBytes);

                return fileName; 
            }
        }
        public string ReadQrCode(string filePath)
        {
            if (!File.Exists(filePath))
                return "File not found!";

            using var bitmap = SKBitmap.Decode(filePath);
            var reader = new BarcodeReader();  
            var result = reader.Decode(bitmap);

            return result?.Text ?? "No QR code found.";
        }
    }
}
