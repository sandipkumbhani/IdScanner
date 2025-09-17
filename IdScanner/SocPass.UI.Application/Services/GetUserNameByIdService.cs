using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Application.Services
{
    public class GetUserNameByIdService : IGetUserNameByIdService
    {
        private readonly IGetUserNameByIdRepository _getUserNameByIdRepository;
        public GetUserNameByIdService(IGetUserNameByIdRepository getUserNameByIdRepository)
        {
            _getUserNameByIdRepository = getUserNameByIdRepository ?? throw new ArgumentNullException(nameof(getUserNameByIdRepository));
        }
        public async Task<User> GetUserNameByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");
            }

            var userName = await _getUserNameByIdRepository.GetUserNameAsync(userId);
            if (userName == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return userName;
        }
    }
}
