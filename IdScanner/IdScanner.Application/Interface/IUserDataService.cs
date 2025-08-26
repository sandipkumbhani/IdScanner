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
    }
}
