using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface IUserRoleService
    {
        Task<UserRole> CreateUserRoleAsync(UserRole userRole);
        Task<List<UserRole>> GetAllUsersRoleAsync();
        Task DeleteUserRoleById(int roleid);
        Task<UserRole> UpdateUserRoleAsync(int roleid, UserRole userRole);
        Task<UserRole> GetUserRoleById(int id);
    }
}
