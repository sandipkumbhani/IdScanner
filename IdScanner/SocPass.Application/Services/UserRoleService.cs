using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<UserRole> CreateUserRoleAsync(UserRole userRole)
        {
            var menuMaster = new UserRole
            {
                Name = userRole.Name,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            return await _userRoleRepository.AddUserRoleAsync(menuMaster);
        }
        public async Task<List<UserRole>> GetAllUsersRoleAsync()
        {
            var users = await _userRoleRepository.GetAllUsersRole();
            return users.Select(user => new UserRole
            {
                UserRoleId = user.UserRoleId,
                Name = user.Name,
                IsActive = user.IsActive,
                InsertBy = user.InsertBy,
                InsertDate = user.InsertDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate,

            }).ToList();
        }
        public async Task DeleteUserRoleById(int roleid)
        {
            var deleteUserRole = await _userRoleRepository.GetUserRoleById(roleid);
            if (deleteUserRole == null)
            {
                throw new KeyNotFoundException($"UserRole ID {roleid} not found.");
            }

            await _userRoleRepository.DeleteRoleAsync(deleteUserRole);
        }
        public async Task<UserRole> UpdateUserRoleAsync(int roleid, UserRole userRole)
        {

            var userRoleExisting = await _userRoleRepository.GetUserRoleById(roleid);

            if (userRoleExisting == null)
            {
                throw new Exception($"UserRole with ID {roleid} not found.");
            }
            userRoleExisting.Name = userRole.Name;
            userRoleExisting.IsActive = true;
            userRoleExisting.InsertBy = 1;
            userRoleExisting.InsertDate = DateTime.UtcNow;
            userRoleExisting.UpdateBy = 1;
            userRoleExisting.UpdateDate = DateTime.UtcNow;


            await _userRoleRepository.UserRoleUpdateAsync(userRoleExisting);

            return userRoleExisting;
        }
        public async Task<UserRole> GetUserRoleById(int id)
        {
            var menuMaster = await _userRoleRepository.GetUserRoleById(id);
            if (menuMaster == null)
            {
                throw new KeyNotFoundException($"User Role with ID {id} not found.");
            }

            return menuMaster;
        }
    }
}
