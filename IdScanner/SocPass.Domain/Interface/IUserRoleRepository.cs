using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddUserRoleAsync(UserRole userRole);
        Task<List<UserRole>> GetAllUsersRole();
        Task UserRoleUpdateAsync(UserRole userRole);
        Task DeleteRoleAsync(UserRole userRole);
        Task<UserRole> GetUserRoleById(int roleid);
    }
}
