using SocPass.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Application.Interface
{
    public interface ISocietyService
    {
        Task<Society> CreateSocietyAsync(Society society);
        Task<Society> GetById(int societyId);
        Task<List<Society>> GetSocietyUserIdAsync(int userId);
        Task<List<Society>> GetSocietyAsync();
        Task<Society> UpdateAsync(Society society);
        Task DeleteSocietyById(int society);
    }
}
