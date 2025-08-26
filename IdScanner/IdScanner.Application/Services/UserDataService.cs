using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly GoogleDriveService _googleDriveService;
        public UserDataService(IUserDataRepository userDataRepository, GoogleDriveService googleDriveService)
        {
            _userDataRepository = userDataRepository;
            _googleDriveService = googleDriveService;
        }
        public async Task<UserData> CreateUserDataAsync(UserData userData)
        {
            if (userData == null)
            {
                return null;
            }
            string photoUrl = null;
            string policeUrl = null;
            string medicalUrl = null;

            if (!string.IsNullOrEmpty(userData.PhotoUrl) && File.Exists(userData.PhotoUrl))
            {
                var photoFileName = $"{userData.IdNumber}_photo_{DateTime.UtcNow.Ticks}.jpg";
                using var photoStream = new FileStream(userData.PhotoUrl, FileMode.Open, FileAccess.Read);
                photoUrl = await _googleDriveService.UploadFileAsync(photoStream, photoFileName, "image/jpeg");
            }

            if (!string.IsNullOrEmpty(userData.PoliceVerificationCertificateUrl) && File.Exists(userData.PoliceVerificationCertificateUrl))
            {
                var policeFileName = $"{userData.IdNumber}_police_{DateTime.UtcNow.Ticks}.pdf";
                using var policeStream = new FileStream(userData.PoliceVerificationCertificateUrl, FileMode.Open, FileAccess.Read);
                policeUrl = await _googleDriveService.UploadFileAsync(policeStream, policeFileName, "image/jpeg");
            }

            if (!string.IsNullOrEmpty(userData.MedicalCertificateUrl) && File.Exists(userData.MedicalCertificateUrl))
            {
                var medicalFileName = $"{userData.IdNumber}_medical_{DateTime.UtcNow.Ticks}.pdf";
                using var medicalStream = new FileStream(userData.MedicalCertificateUrl, FileMode.Open, FileAccess.Read);
                medicalUrl = await _googleDriveService.UploadFileAsync(medicalStream, medicalFileName, "image/jpeg");
            }
            var newData = new UserData
            {
                CompanyId = userData.CompanyId,
                DepartmentId = userData.DepartmentId,
                Name = userData.Name,
                Designation = userData.Designation,
                IdNumber = userData.IdNumber,
                MobileNumber = userData.MobileNumber,
                StallPfNumber = userData.StallPfNumber,
                Licensee = userData.Licensee,
                WorkSlot = userData.WorkSlot,
                IdValidTill = userData.IdValidTill,
                PhotoUrl = photoUrl,
                PoliceVerificationCertificateUrl = policeUrl,
                MedicalCertificateUrl = medicalUrl,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            return await _userDataRepository.AddUserDataAsync(newData);
        }

    }
}
