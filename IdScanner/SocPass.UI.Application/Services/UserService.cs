using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserAdapter _userRepository;
        public UserService(IUserAdapter userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IList<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUsersByIdAsync(userId);
        }
        public async Task<User> AddUserAsync(User user, int? flatId = null)
        {
            return await _userRepository.AddUserAsync(user, flatId);
        }
        public async Task<User> UpdateUserAsync(User model, int? flatId = null)
        {
            return await _userRepository.UpdateUserAsync(model, flatId);
        }
        public async Task<string> Deleteuserasync(int userid)
        {
            return await _userRepository.DeleteUserAsync(userid);
        }
        //UsrRole
        public async Task<IList<UserRole>> GetAllUserRoleAsync()
        {
            return await _userRepository.GetAllUserRoleAsync();
        }
        public async Task<UserRole> GetRoleNameByIdAsync(long? id)
        {
            return await _userRepository.GetRoleNameByIdAsync(id);
        }
    }
}
