using Microsoft.Extensions.Configuration;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class MemberAdapter : IMemberAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;
        private readonly ICommonAdapter _commonAdapter;
        public MemberAdapter(HttpClient httpCleint, IConfiguration configuration, GlobalClass globalClass, ICommonAdapter commonAdapterl)
        {
            _httpClient = httpCleint;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
            _commonAdapter = commonAdapterl;
        }
        public async Task<IList<Member>> GetAllGuestAsync(int flatId)
        {
            return await _commonAdapter.GetAsync<IList<Member>>("Member/GetGuestByid?flatId={flatId}");
        }
        public async Task<IList<Member>> GetAllMemberAsync(int flatId)
        {
            return await _commonAdapter.GetAsync<IList<Member>>("Member/GetMemberByid?flatId={flatId}");
        }
        public async Task<string> AddMemberAsync(MemberCreateRequest memberCreateRequest)
        {
            return await _commonAdapter.PutAsync("Member/add-update-member", memberCreateRequest);
        }
        public async Task<string> AddAndUpdateGuestAsync(MemberCreateRequest memberCreateRequest)
        {
            return await _commonAdapter.PutAsync("Member/Update-Guest", memberCreateRequest);
        }
        public async Task<string> GenerateMemberPass(int blockId, int EventId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = $"{apiCredential.url}Member/AddPassdate?blockId={blockId}&EventId={EventId}";
            var response = await _httpClient.PutAsync(baseUrl, null); 
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GenerateGuestPass(int blockId, int EventId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = $"{apiCredential.url}Member/AddGuestPassdate?blockId={blockId}&EventId={EventId}";
            var response = await _httpClient.PutAsync(baseUrl, null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<QRCodeMaster> GetMemberByMemberId(int? memberId,int EventId)
        {
            return await _commonAdapter.PostAsync<QRCodeMaster>($"Member/GetMemberByMemberId?memberId={memberId}&EventId={EventId}");
        }
    }
}
