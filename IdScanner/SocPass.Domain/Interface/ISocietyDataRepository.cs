using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;


namespace SocPass.Domain.Interface
{
    public interface ISocietyDataRepository
    {
        Task<List<SocietyData>> GetSocietyDataAsync();
        Task<SocietyData> AddSocietyDataAsync(SocietyData societyData);
        Task<SocietyData> GetSocietyDataByIdAsync(int? SocietyDataId);
        Task UpdateSocietyAsync(SocietyData societyData);
        Task DeleteSocietyAsync(int societyDataId);

        Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId);

    }
}

