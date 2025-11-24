using SocPass.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface ISocietyService
    {
        Task<IList<Society>> GetSocietyAsync();
        Task<Society> GetSocietyByIdAsync(int? societyId);
        Task<string> AddSocietyAsync(Society society);
        Task<string> UpdateSocietyAsync(Society society);
        Task<string> DeleteSocietyAsync(int societyId);
    }
}
