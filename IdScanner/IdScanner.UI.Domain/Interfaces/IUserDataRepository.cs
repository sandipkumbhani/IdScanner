using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IUserDataRepository
    {
        Task<List<UserData>> GetAllUserDetailsAsync();
        Task<UserData> AddUserDataAsync(UserData userData);
        Task<UserData> GetUserDataByIdAsync(int? userid);
    }
}
