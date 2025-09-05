using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Interface
{
    public interface IQrCodeService
    {
        string SaveQrCode(string text, string folderPath);
        string ReadQrCode(string filePath);
    }
}
