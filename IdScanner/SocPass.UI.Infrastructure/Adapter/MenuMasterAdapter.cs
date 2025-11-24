using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

                return response?.Message ?? "Menu added successfully.";
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to connect to the server: {ex.Message}";
            }
            catch (JsonException ex)
            {
                return $"Invalid response format from server: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
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

