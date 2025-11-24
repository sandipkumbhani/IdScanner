using SocPass.Domain.DTO;
using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface ISocietyDataService
    {
        Task<IList<SocietyData>> GetSocietyDataAsync();
        Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId);
        Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData);
        Task<string> UpdateSocietyDataAsync(SocietyData societyData);
        Task<string> DeleteSocietyDataAsync(int societyDataId);
        //Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId);
    }
}
