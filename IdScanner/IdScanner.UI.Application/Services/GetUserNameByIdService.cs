using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.UI.Application.Services
{
    public class GetUserNameByIdService : IGetUserNameByIdService
    {
        private readonly IGetUserNameByIdRepository _getUserNameByIdRepository;
        public GetUserNameByIdService(IGetUserNameByIdRepository getUserNameByIdRepository)
        {
            _getUserNameByIdRepository = getUserNameByIdRepository ?? throw new ArgumentNullException(nameof(getUserNameByIdRepository));
        }
        public async Task<User> GetUserNameByIdAsync(long userId)
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
