using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Interfaces;

namespace SocPass.UI.Infrastructure.Provider
{
    public class MenuMasterAdapter : IMenuMasterAdapter
    {
        private readonly ICommonAdapter _commonAdapter;
        public MenuMasterAdapter(ICommonAdapter commonAdapter)
        {
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<MenuMaster>> GetAllMenuAsync()
        {
            return await _commonAdapter.GetAsync<IList<MenuMaster>>("MenuMaster/Get-All-Menu-Master");
        }
        public async Task<MenuMaster> GetMenuByIdAsync(int? id)
        {
            return await _commonAdapter.GetAsync<MenuMaster>($"MenuMaster/GetByMenuId?id={id}");
        }
        public async Task<string> AddMenuAsync(MenuMaster menuMaster)
        {
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<MenuMaster>>("MenuMaster/Menu-Master", menuMaster);

                if (response == null)
                    return "Unexpected null response from API.";

                if (response.IsSuccess)
                    return response.Message ?? "Menu added successfully.";
                return response.Message ?? "Failed to create Menu.";
            }
            catch (HttpRequestException ex)
            {
                var statusCode = ex.StatusCode?.ToString() ?? "Unknown";
                var errorContent = ex.Message;
                if (statusCode == "Conflict")
                {
                    return $"Menu Order '{menuMaster.MenuOrder}' already exists.";
                }

                return $"Failed to create block. Server responded with {statusCode}: {errorContent}";
            }
            catch (Exception ex)
            {
                return $"Unexpected error occurred while creating block: {ex.Message}";
            }
        }
        public async Task<string> UpdateMenuAsync(MenuMaster menuMaster)
        {
            return await _commonAdapter.PutAsync($"MenuMaster/Update-Menu", menuMaster);
        }
        public async Task<string> DeleteMenuAsync(int id)
        {
            return await _commonAdapter.DeleteAsync<string>($"MenuMaster/Delete-Menu-Master?id={id}");
        }
        //public async Task<IList<MenuMaster>> GetMenuByUserIdAsync(int userId)
        //{
        //    return await _commonAdapter.GetAsync<IList<MenuMaster>>($"MenuMaster/GetMenusByUserId?userId={userId}");
           
        //}
    }
}

