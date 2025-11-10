using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.UI.Domain.Interfaces
{
    public interface ISocietyDataAdapter
    {
        Task<IList<SocietyData>> GetAllSocietyData();
        Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId);
        Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData);
        Task<string> UpdateSocietyDataAsync(SocietyData societyData);
        Task<string> DeleteSocietyDataAsync(int societyDataId);
        //Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId);

    }
}
