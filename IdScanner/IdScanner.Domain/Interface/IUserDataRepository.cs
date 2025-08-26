using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Interface
{
    public interface IUserDataRepository
    {
        Task<UserData> AddUserDataAsync(UserData UserData);
    }
}
