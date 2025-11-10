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
        Task<User> CreateUserAsync(User user, int? flatId = null);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserById(int id);
        Task<User> UpdateUserAsync(User user, int? flatId = null);
        Task<User?> GetUserDetailsById(int userid);
    }
}
