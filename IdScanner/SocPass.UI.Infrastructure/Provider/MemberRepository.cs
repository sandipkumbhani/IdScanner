using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
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
    public class MemberRepository : IMemberRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        public MemberRepository(HttpClient httpCleint, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<List<Member>> GetAllGuestAsync(int flatId)
        {
            var baseUrl = apiCredential.url + "Member/GetGuestByid?flatId={flatId}";
            var response = await _httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Member>>(json)!;
        }
        public async Task<List<Member>> GetAllMemberAsync(int flatId)
        {
            var baseUrl = apiCredential.url + $"Member/GetMemberByid?flatId={flatId}";
            var response = await _httpClient.GetAsync(baseUrl);
                response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Member>>(json)!;
        }
        public async Task<string> AddMemberAsync(MemberCreateRequest memberCreateRequest)
        {
            var baseUrl = apiCredential.url + $"Member/add-update-member";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(memberCreateRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> AddAndUpdateGuestAsync(MemberCreateRequest memberCreateRequest)
        {
            var baseUrl = apiCredential.url + $"Member/Update-Guest";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(memberCreateRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GeneratePass(int blockId, DateTime passDate)
        {
            var baseUrl = $"{apiCredential.url}Member/AddPassdate?blockId={blockId}&passDate={passDate:O}";
            var response = await _httpClient.PutAsync(baseUrl, null); 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GenerateGuestPass(int blockId, DateTime passDate)
        {
            var baseUrl = $"{apiCredential.url}Member/AddGuestPassdate?blockId={blockId}&passDate={passDate:O}";
            var response = await _httpClient.PutAsync(baseUrl, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<Member> GetMemberByMemberId(int? memberId)
        {
            var baseUrl = apiCredential.url + $"Member/GetMemberByMemberId?memberId={memberId}";
            var response = await _httpClient.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Member>(jsonString)!;
        }
    }
}
