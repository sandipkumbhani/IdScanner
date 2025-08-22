using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Domain.Interface
{
    public interface IUserRoleRepository
    {

        Task<UserRole> AddUserRoleAsync(UserRole userRole);
        Task<List<UserRole>> GetAllUsersRole();
        Task DeleteRoleAsync(UserRole userRole);
        Task UserRoleUpdateAsync(UserRole userRole);
        Task<UserRole> GetUserRoleById(int roleid);
    }
}
