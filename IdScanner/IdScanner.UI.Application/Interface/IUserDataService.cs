using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Interface
{
    public interface IUserDataService
    {
        Task<List<UserData>> GetAllUserDetailsAsync();
        Task<UserData> AddUserDataAsync(UserData userData);
        Task<UserData> GetById(int userid);
        Task<string> UpdateUserAsync(UserData userData);    
        Task<string> DeleteUserAsync(int id);
        Task<string> UpdateQrCodeAsync(long userid, string qrCodeUrl);

	}
}
