using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserById(int id);
        Task DeleteAsync(int id);
        Task UserUpdateAsync(User user);
    }
}
