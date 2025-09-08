using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _userDataRepository;
        public UserDataService(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }
        public async Task<List<UserData>> GetAllUserDetailsAsync()
        {
            return await _userDataRepository.GetAllUserDetailsAsync();
        }
        public async Task<UserData> AddUserDataAsync(UserData userData)
        {
            return await _userDataRepository.AddUserDataAsync(userData);

        }
        public async Task<UserData> GetById(int userid)
        {
            return await _userDataRepository.GetUserDataByIdAsync(userid);
        }

        public async Task<string> UpdateUserAsync(UserData userData)
        {
            return await _userDataRepository.UpdateUserAsync(userData);
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            return await _userDataRepository.DeleteUserAsync(id);
        }
        public async Task<string> UpdateQrCodeAsync(long userid,string qrCodeUrl)
        {
            return await _userDataRepository.UpdateQrCodeAsync(userid, qrCodeUrl);
        }


	}
}
