using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Interface
{
    public interface IUserDataService
    {
        Task<UserData> CreateUserDataAsync(UserData userData);
        Task<UserData> UpdateUserDataAsync(int id, UserData userData);
        Task<UserData> GetUserDetailsById(int userid);
        Task<List<UserData>> GetAllUsersListAsync();
        Task DeleteUserDataById(int userId);
        Task UpdateQrCodeAsync(long userId, string qrCodeUrl);

	}
}
