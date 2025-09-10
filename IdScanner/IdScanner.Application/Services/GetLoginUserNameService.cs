using IdScanner.Application.Interface;
using IdScanner.Domain.Interface;
using IdScanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdScanner.Application.Services
{
    public class GetLoginUserNameService : IGetLoginUserNameService
    {
        private readonly IGetLoginUserNameRepository _getLoginUserNameRepository;
        public GetLoginUserNameService(IGetLoginUserNameRepository getLoginUserNameRepository)
        {
            _getLoginUserNameRepository = getLoginUserNameRepository;
        }
        public async Task<User> GetLoginUserNameAsync(long userId)
        {
            return await _getLoginUserNameRepository.GetUserNameAsync(userId);
        }
    }
}
