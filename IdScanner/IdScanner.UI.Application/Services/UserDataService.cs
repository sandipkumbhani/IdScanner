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

    }
}
