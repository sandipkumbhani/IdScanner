using Microsoft.Extensions.Configuration;
using SocPass.Domain.DTO;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class SocietyDataAdapter : ISocietyDataAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        private readonly ICommonAdapter _commonAdapter;

        public SocietyDataAdapter(HttpClient httpCleint, IConfiguration configuration, GlobalClass globalClass, ICommonAdapter commonAdapter)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<SocietyData>> GetAllSocietyData()
        {
            return await _commonAdapter.GetAsync<IList<SocietyData>>($"SocietyData/Get-All-SocietyData");
        }
        public async Task<SocietyData> GetSocietyDataByIdAsync(int? societyDataId)
        {
            return await _commonAdapter.GetAsync<SocietyData>($"SocietyData/GetBySocietyDataId?societyDataId={societyDataId}");
        }
        public async Task<string> AddSocietyDataAsync(SocietyDataCreateRequest societyData)
        {
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<SocietyDataCreateRequest>>("SocietyData/Add-Societydata", societyData);

                return response?.Message ?? "Society Data added successfully.";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
        public async Task<string> UpdateSocietyDataAsync(SocietyData societyData)
        {
            return await _commonAdapter.PutAsync($"SocietyData/Update-Societydata", societyData);
        }
        public async Task<string> DeleteSocietyDataAsync(int societyDataId)
        {
            return await _commonAdapter.DeleteAsync<string>($"SocietyData/Delete-SocietyData?societyDataId={societyDataId}");
        }

        //public async Task<List<SocietyData>> GetSocietyDataByFlatId(int flatId)
        //{
        //    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
        //    var baseUrl = apiCredential.url + $"SocietyData/Get-SocietyData-By-FlatId?flatId={flatId}";
        //    var response = await _httpClient.GetAsync(baseUrl);
        //    response.EnsureSuccessStatusCode();
        //    var json = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<List<SocietyData>>(json)!;
        //}
    }
}
