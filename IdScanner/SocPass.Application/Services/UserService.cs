using Microsoft.AspNetCore.Http;
using SocPass.Application.Interface;
using SocPass.Domain.Interface;
using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserFlatMappingRepository _userFlatMappingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISocietyRepository _societyRepository;
        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, 
            IUserFlatMappingRepository userFlatMappingRepository, IHttpContextAccessor httpContextAccessor, ISocietyRepository societyRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userFlatMappingRepository = userFlatMappingRepository;
            _httpContextAccessor = httpContextAccessor;
            _societyRepository = societyRepository;
        }
        public async Task<User> CreateUserAsync(User user, int? flatId = null)
        {
            bool emailExists = await _userRepository.EmailExistsAsync(user.EmailId);
            if (emailExists)
            {
                throw new EmailAlreadyExistsException("This email is already registered.");
            }
            var role = await _userRoleRepository.GetUserRoleById(user.UserRoleId);
            int? societyId = role.Name == "Admin" ? null : user.SocietyId;

            if (role.Name == "User")
            {
                var checkFlatExsiting = await _userFlatMappingRepository.GetMappingByFlatId(flatId);
                if (checkFlatExsiting != null && checkFlatExsiting.FlatId == flatId)
                {
                    throw new FlatAlreadyExistsException("Flat already exists.");
                }
            }

                var newUser = new User
            {
                Name = user.Name,
                EmailId = user.EmailId,
                Password = user.Password,
                UserRoleId = user.UserRoleId,
                SocietyId = societyId,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            var result = await _userRepository.AddUserAsync(newUser);
            if (role.Name == "User")
            {
                var userMapping = new UserFlatMapping
                {
                    UserId = result.UserId,
                    FlatId = flatId.Value,
                    IsActive = true,
                    InsertBy = 1,
                    InsertDate = DateTime.Now,
                    UpdateBy = 1,
                    UpdateDate = DateTime.Now
                };
                await _userFlatMappingRepository.AddFlatMappingAsync(userMapping);
            }
            return result;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            List<User> userList = new List<User>();

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                userList = await _userRepository.GetUsersAsync();
            }
            else
            {
                var societies = await _societyRepository.GetSocietyAsync(userId);
                var assignedSociety = societies.FirstOrDefault();

                if (assignedSociety != null)
                {
                    userList = (await _userRepository.GetUsersAsync())
                                .Where(u => u.SocietyId == assignedSociety.SocietyId
                                         && u.UserRole.Name != "Society")
                                .ToList();
                }
            }

            return userList;
        }

        public async Task DeleteUserById(int id)
        {
            var deleteUser = await _userRepository.GetUserById(id);
            if (deleteUser == null)
            {
                throw new KeyNotFoundException($"User ID {id} not found.");
            }

            await _userRepository.DeleteAsync(id);
        }
        public async Task<User> UpdateUserAsync(User user, int? flatId = null)
        {
            var role = await _userRoleRepository.GetUserRoleById(user.UserRoleId);

            var userExisting = await _userRepository.GetUserById(user.UserId);

            if (userExisting == null)
            {
                throw new Exception($"User with ID {user.UserId} not found.");
            }
            userExisting.Name = user.Name;
            userExisting.EmailId = user.EmailId;
            userExisting.UserRoleId = user.UserRoleId;
            userExisting.SocietyId = user.SocietyId;
            userExisting.IsActive = true;
            userExisting.InsertBy = 1;
            userExisting.InsertDate = DateTime.UtcNow;
            userExisting.UpdateBy = 1;
            userExisting.UpdateDate = DateTime.UtcNow;
            userExisting.Password = user.Password;
            await _userRepository.UserUpdateAsync(userExisting);

            if (role.Name == "User" && flatId.HasValue)
            {
                var existingMapping = await _userFlatMappingRepository.GetMappingByUserId(user.UserId);

                if (existingMapping != null)
                {
                    existingMapping.FlatId = flatId.Value;
                    existingMapping.UpdateBy = 1;
                    existingMapping.UpdateDate = DateTime.UtcNow;
                    await _userFlatMappingRepository.UpdateFlatMappingAsync(existingMapping);
                }
            }
            return userExisting;
        }
        public async Task<User?> GetUserDetailsById(int userid)
        {
            var UserDetails =await _userRepository.GetUserById(userid);
            if (UserDetails == null)
            {
                throw new KeyNotFoundException($"User Id with ID {userid} not found.");
            }
            return UserDetails;
        }
        public class EmailAlreadyExistsException : Exception
        {
            public EmailAlreadyExistsException(string message) : base(message) { }
        }

        public class FlatAlreadyExistsException : Exception
        {
            public FlatAlreadyExistsException(string message) : base(message) { }
        }


    }
}
