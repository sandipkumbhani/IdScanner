using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IUserFlatMappingRepository
    {
        Task<UserFlatMapping> AddFlatMappingAsync(UserFlatMapping userFlatMapping);
        Task<List<Member>> GetMembersByUserIdAsync(int loginUserId);
        Task UpdateFlatMappingAsync(UserFlatMapping userFlatMapping);
        Task<UserFlatMapping> GetMappingByUserId(int userid);
        Task<UserFlatMapping> GetMappingByFlatId(int? flatId);
    }
}
