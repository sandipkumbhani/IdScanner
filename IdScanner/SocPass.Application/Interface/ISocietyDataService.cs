using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.Application.Interface
{
    public interface ISocietyDataService
    {
        Task CreateSocietyDataAsync(SocietyDataCreateRequest request);
        Task<SocietyData> GetSocietyDataByIdAsync(int societyDataId);
        Task<SocietyData>UpdateSocietyDataAsync(SocietyData request);
        Task DeleteSocietyDataByIdAsync(int societyDataId);
        Task<List<SocietyData>> GetAllSocietyDataAsync();
        Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId);
    }
}
