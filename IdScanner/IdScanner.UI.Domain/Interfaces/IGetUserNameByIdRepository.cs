using Google.Apis.Drive.v3.Data;
using IdScanner.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = IdScanner.Domain.Model.User;

namespace IdScanner.UI.Domain.Interfaces
{
    public interface IGetUserNameByIdRepository
    {
        Task<User> GetUserNameAsync(long userId);
    }
}
