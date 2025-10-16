using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface ISocietyService
    {
        Task<List<Society>> GetAllSocietyAsync(int userId);
        Task<List<Society>> GetAllSocietyAsync();
        Task<Society> GetSocietyByIdAsync(int? societyId);
        Task<string> AddSocietyAsync(Society society);
        Task<string> UpdateSocietyAsync(Society society);
        Task<string> DeleteSocietyAsync(int societyId);
    }
}
