using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface ISocietyRepository
    {
        Task<Society> CreateSocietyAsync(Society society);
        Task<List<Society>> GetAllSocietyAsync(int userId);
        Task<List<Society>> GetAllSocietyAsync();
        Task<Society> GetByIdAsync(int societyid);
        Task UpdateSocietyAsync(Society society);
        Task DeleteSocietyAsync(Society society);
    }
}
