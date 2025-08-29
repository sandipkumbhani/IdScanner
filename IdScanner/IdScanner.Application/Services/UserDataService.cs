using Google.Apis.Drive.v3.Data;
using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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

            var idNumber = Generate12Digit();
            string folderId = await _googleDriveService.CreateFolderAsync(idNumber);

            if (!string.IsNullOrEmpty(userData.PhotoUrl) && System.IO.File.Exists(userData.PhotoUrl))
            {
                string fileExtension = Path.GetExtension(userData.PhotoUrl)?.ToLower();
                string mimeType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    _ => throw new NotSupportedException($"Unsupported file type: {fileExtension}")
                };
                var photoFileName = $"{idNumber}_photo{fileExtension}"; 

                using var photoStream = new FileStream(
                    userData.PhotoUrl,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

                photoUrl = await _googleDriveService.UploadFileAsync(photoStream, photoFileName, "image/jpeg", folderId);
            }
            if (!string.IsNullOrEmpty(userData.PoliceVerificationCertificateUrl) && System.IO.File.Exists(userData.PoliceVerificationCertificateUrl))
            {
                string fileExtension = Path.GetExtension(userData.PoliceVerificationCertificateUrl)?.ToLower();
                string mimeType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    _ => throw new NotSupportedException($"Unsupported file type: {fileExtension}")
                };
                var policeFileName = $"{idNumber}_police{fileExtension}";
                using var policeStream = new FileStream(
                    userData.PoliceVerificationCertificateUrl,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);
                //new FileStream(userData.PoliceVerificationCertificateUrl, FileMode.Open, FileAccess.Read);
                policeUrl = await _googleDriveService.UploadFileAsync(policeStream, policeFileName, "image/jpeg", folderId);
            }
            if (!string.IsNullOrEmpty(userData.MedicalCertificateUrl) && System.IO.File.Exists(userData.MedicalCertificateUrl))
            {
                string fileExtension = Path.GetExtension(userData.MedicalCertificateUrl)?.ToLower();
                string mimeType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    _ => throw new NotSupportedException($"Unsupported file type: {fileExtension}")
                };
                var medicalFileName = $"{idNumber}_medical{fileExtension}"; ;

                using var medicalStream = new FileStream(
                    userData.MedicalCertificateUrl,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);
                //new FileStream(userData.MedicalCertificateUrl, FileMode.Open, FileAccess.Read);
                medicalUrl = await _googleDriveService.UploadFileAsync(medicalStream, medicalFileName, "image/jpeg", folderId);
            }

            var newData = new UserData
            {
                CompanyId = userData.CompanyId,
                DepartmentId = userData.DepartmentId,
                Name = userData.Name,
                Designation = userData.Designation,
                IdNumber = idNumber,
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
        public async Task<UserData> UpdateUserDataAsync(int id, UserData userData)
        {
            var userExisting = await _userDataRepository.GetUserDataById(id);
            if (userExisting == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }
            string newphotoUrl = userData.PhotoUrl;
            string newPolice = userData.PoliceVerificationCertificateUrl;
            string newMedical = userData.MedicalCertificateUrl;
            string folderId = userExisting.IdNumber;
            string photoUrl = userExisting.PhotoUrl;
            string policeUrl = userExisting.PoliceVerificationCertificateUrl;
            string medicalUrl = userExisting.MedicalCertificateUrl;

            photoUrl = await CreateOrUpdateFileOnDrive(photoUrl, folderId, newphotoUrl);
            policeUrl = await CreateOrUpdateFileOnDrive(policeUrl, folderId, newPolice);
            medicalUrl = await CreateOrUpdateFileOnDrive(medicalUrl, folderId, newMedical);
            
            userExisting.CompanyId = userData.CompanyId;
            userExisting.DepartmentId = userData.DepartmentId;
            userExisting.Name = userData.Name;
            userExisting.MobileNumber = userData.MobileNumber;
            userExisting.StallPfNumber = userData.StallPfNumber;
            userExisting.Licensee = userData.Licensee;
            userExisting.WorkSlot = userData.WorkSlot;
            userExisting.IdValidTill = userData.IdValidTill;
            userExisting.PhotoUrl = photoUrl;
            userExisting.PoliceVerificationCertificateUrl = policeUrl;
            userExisting.MedicalCertificateUrl = medicalUrl;
            userExisting.IsActive = true;
            userExisting.UpdateBy = 1; 
            userExisting.UpdateDate = DateTime.Now;

            await _userDataRepository.UpdateUserDataAsync(userExisting);
            return userExisting;
        }
        public async Task<UserData> GetUserDetailsById(int userid)
        {
            var userDetails = await _userDataRepository.GetUserDataById(userid);
            if (userDetails == null)
            {
                throw new KeyNotFoundException($"User with ID {userid} not found.");
            }

            return userDetails;
        }   
    public async Task<List<UserData>> GetAllUsersListAsync()
        {
            var users = await _userDataRepository.GetAllUserDataAsync();
            return users.Select(User => new UserData
            {
                UserDataId = User.UserDataId,
                CompanyId = User.CompanyId,
                DepartmentId = User.DepartmentId,
                Name = User.Name,
                Designation = User.Designation,
                IdNumber = User.IdNumber,
                MobileNumber = User.MobileNumber,
                StallPfNumber = User.StallPfNumber,
                Licensee = User.Licensee,
                WorkSlot = User.WorkSlot,
                IdValidTill = User.IdValidTill,
                PhotoUrl = User.PhotoUrl,
                PoliceVerificationCertificateUrl = User.PoliceVerificationCertificateUrl,
                MedicalCertificateUrl = User.MedicalCertificateUrl,
                IsActive = User.IsActive,
                InsertBy = User.InsertBy,
                InsertDate = User.InsertDate,
                UpdateBy = User.UpdateBy,
                UpdateDate = User.UpdateDate,
                CompanyMaster = User.CompanyMaster,
                Department = User.Department 

            }).ToList();
        }
        public async Task DeleteUserDataById(int userId)
        {
            var deleteUser = _userDataRepository.GetUserDataById(userId);
            if (deleteUser == null)
            {
                throw new KeyNotFoundException($"User ID {userId} not found.");
            }
            await _userDataRepository.DeleteAsync(deleteUser.Result);
        }
        private static string Generate12Digit()
        {
            long ticks = DateTime.UtcNow.Ticks; 
            string tickPart = ticks.ToString().Substring(ticks.ToString().Length - 9);
            string randomPart = new Random().Next(100, 999).ToString(); 
            return tickPart + randomPart;
        }
        private async Task<string> CreateOrUpdateFileOnDrive(string ImageURL, string FolderId, string NewphotoUrl)
        {
            string FileName = "";
            if (!string.IsNullOrEmpty(NewphotoUrl) && System.IO.File.Exists(NewphotoUrl))
            {
                string fileExtension = Path.GetExtension(NewphotoUrl)?.ToLower();
                string mimeType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    _ => throw new NotSupportedException($"Unsupported file type: {fileExtension}")
                };
                var FileNameWithExtenstion = $"{FolderId}_photo{fileExtension}";

                using var FileStream = new FileStream(
                    NewphotoUrl,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

                string existingFileId = await _googleDriveService.GetFileIdByNameAsync(FolderId, ImageURL);
                if (!string.IsNullOrEmpty(existingFileId))
                {
                    FileName = await _googleDriveService.UpdateFileAsync(existingFileId, FileStream, mimeType);
                }
                else
                {
                    FileName = await _googleDriveService.UploadFileAsync(FileStream, FileNameWithExtenstion, mimeType, FolderId);
                }

            }
            return FileName;
        }
        
    }
}
