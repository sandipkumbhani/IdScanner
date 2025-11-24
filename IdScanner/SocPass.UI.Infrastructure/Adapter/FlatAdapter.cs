using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocPass.Domain.DTO;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class FlatAdapter :IFlatAdapter
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        private readonly ICommonAdapter _commonAdapter;

        public FlatAdapter(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass, ICommonAdapter commonAdapter)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapter;
        }

        public async Task<IList<Flat>> GetFlatAsync()
        {
            return await _commonAdapter.GetAsync<IList<Flat>>("Flat/GetFlat");
        }

        public async Task<IList<Flat>> GetFlatByIdAsync(int? societyId, int blockId)
        {
            return await _commonAdapter.GetAsync<IList<Flat>>("$Flat/GetFlatById?societyId={societyId}&blockId={blockId}");
         
        }
        public async Task<List<Flat>> UpdateFlatAsync(Flat flat)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "Flat/Update-flat";
            var flatJson = JsonConvert.SerializeObject(flat);
            var requestContent = new StringContent(flatJson, Encoding.UTF8, "application/json");

            var response = await _httpClinet.PutAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseData);
                return result?.data?.ToObject<List<Flat>>() ?? new List<Flat>();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                try
                {
                    var error = JsonConvert.DeserializeObject<dynamic>(responseData);
                    string message = error?.message ?? "Flat already exists.";
                    throw new InvalidOperationException(message);
                }
                catch
                {
                    throw new InvalidOperationException("Flat already exists.");
                }
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = JsonConvert.DeserializeObject<dynamic>(responseData);
                string message = error?.message ?? "Invalid flat data.";
                throw new InvalidOperationException(message);
            }
            throw new Exception($"API Error ({response.StatusCode}): {responseData}");
        }


        public async Task<IList<Flat>> GetFlatByBlockId(int blockid)
        {
            return await _commonAdapter.GetAsync<IList<Flat>>($"Flat/GetFlatByBlockid?blockid={blockid}");
        }
        public async Task<IList<FlatWithMembersDto>> GetQR(int blockid, int eventId)
        {
            return await _commonAdapter.GetAsync<IList<FlatWithMembersDto>>($"Flat/GetQR?blockid={blockid}&eventId={eventId}");
        }
        public async Task<IList<FlatWithMembersDto>> GetGuestQR(int blockid, int EventId)
        {
            return await _commonAdapter.GetAsync<IList<FlatWithMembersDto>>($"Flat/GetGuestQR?blockid={blockid}&EventId={EventId}");
        }
    }
}
