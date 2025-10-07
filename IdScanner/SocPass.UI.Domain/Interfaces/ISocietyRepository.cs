using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocPass.Domain.Model;

namespace SocPass.UI.Domain.Interfaces
{
    public interface ISocietyRepository
    {
        Task<List<Society>> GetAllSocietyAsync(int userId);
        Task<List<Society>> GetAllSocietyAsync();
        Task<Society> GetSocietyByIdAsync(int? societyId);
        Task<string> AddSocietyAsync(Society society);
        Task<string> UpdateSocietyAsync(Society society);
        Task<string> DeleteSocietyAsync(int societyId);
    }
}
