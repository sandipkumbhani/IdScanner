using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(long userId)
        {
            return await _userRepository.GetUsersByIdAsync(userId);
        }
        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.AddUserAsync(user);
        }
        public async Task<User> UpdateUserAsync(User model)
        {
            return await _userRepository.UpdateUserAsync(model);
        }
        public async Task<string> Deleteuserasync(int userid)
        {
            return await _userRepository.DeleteUserAsync(userid);
        }
        //UsrRole
        public async Task<List<UserRole>> GetAllUserRoleAsync()
        {
            return await _userRepository.GetAllUserRoleAsync();
        }
        public async Task<UserRole> GetRoleNameByIdAsync(long? id)
        {
            return await _userRepository.GetRoleNameByIdAsync(id);
        }
    }
}
