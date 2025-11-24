using Microsoft.Extensions.Configuration;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SocietyAdapter : ISocietyAdapter
    {
        private readonly ICommonAdapter _commonAdapter;

        public SocietyAdapter(ICommonAdapter commonAdapter)
        {
            _commonAdapter = commonAdapter;
        }
        
        public async Task<IList<Society>> GetSocietyByUserId(int userId)
        {
            return await _commonAdapter.GetAsync<IList<Society>>($"Society/GetSocietyByUserId?userId={userId}");
        }

        public async Task<IList<Society>> GetSocietyAsync()
        {
            return await _commonAdapter.GetAsync<IList<Society>>($"Society/getSociety");
        }   

        public async Task<Society> GetSocietyByIdAsync(int? societyId)
        {
            return await _commonAdapter.GetAsync<Society>($"Society/GetSocietyById?societyId={societyId}");
        }
        public async Task<string> AddSocietyAsync(Society society)
        {   
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<Society>>("Society/create", society);
                return response?.Message ?? "Society added successfully.";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
        public async Task<string> UpdateSocietyAsync(Society society)
        {
            return await _commonAdapter.PutAsync($"Society/Update-society", society);
            
        }
        public async Task<string> DeleteSocietyAsync(int societyId)
        {
            return await _commonAdapter.DeleteAsync<string>($"Society/Delete-Society?societyId={societyId}");
        }

    }
}
