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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            bool emailExists = await _userRepository.EmailExistsAsync(user.EmailId);
            if (emailExists)
            {
                throw new InvalidOperationException("This email is already registered.");
            }
            var salt = Guid.NewGuid().ToString("N").Substring(0, 8);
            var newUser = new User
            {
                Name = user.Name,
                EmailId = user.EmailId,
                Password = user.Password,
                UserRoleId = user.UserRoleId,
                SocietyId=user.SocietyId,
                IsActive = true,
                InsertBy = 1,
                InsertDate = DateTime.Now,
                UpdateBy = 1,
                UpdateDate = DateTime.Now
            };
            return await _userRepository.AddUserAsync(newUser);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new User
            {
                UserId = user.UserId,
                Name = user.Name,
                EmailId = user.EmailId,
                UserRoleId = user.UserRoleId,
                SocietyId = user.SocietyId,
                IsActive = user.IsActive,
                InsertBy = user.InsertBy,
                InsertDate = user.InsertDate,
                UpdateBy = user.UpdateBy,
                UpdateDate = user.UpdateDate,
                UserRole = user.UserRole

            }).ToList();
        }
        public async Task DeleteUserById(int id)
        {
            var deleteUser = _userRepository.GetUserById(id);
            if (deleteUser == null)
            {
                throw new KeyNotFoundException($"User ID {id} not found.");
            }

            await _userRepository.DeleteAsync(deleteUser);
        }
        public async Task<User> UpdateUserAsync(int userid, User user)
        {
            var userExisting = _userRepository.GetUserById(userid);

            if (userExisting == null)
            {
                throw new Exception($"User with ID {userid} not found.");
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


            await _userRepository.UserUpdateAsync(userExisting);

            return userExisting;
        }
        public User GetUserDetailsById(int userid)
        {
            var UserDetails = _userRepository.GetUserById(userid);
            if (UserDetails == null)
            {
                throw new KeyNotFoundException($"User Id with ID {userid} not found.");
            }
            return UserDetails;
        }
      
    }
}
