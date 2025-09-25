using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserById(int id);
        Task<User> UpdateUserAsync(int userid, User user);
        User GetUserDetailsById(int userid);
    }
}
