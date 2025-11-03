using Microsoft.Extensions.Logging;
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
    public class UserFlatMappingService : IUserFlatMappingService
    {
        private readonly IUserFlatMappingRepository _userFlatMappingRepository;
        public UserFlatMappingService(IUserFlatMappingRepository userFlatMappingRepository)
        {
            _userFlatMappingRepository = userFlatMappingRepository;
        }
        public async Task<List<QRCodeMaster>> GetQrByUserId(int usereId, int? eventId = null)
        {
            return await _userFlatMappingRepository.GetQrByUserId(usereId, eventId);
        }
    }
}
