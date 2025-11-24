using Microsoft.Extensions.Configuration;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Infrastructure.Provider
{
    public class MemberDetailsAdapter : IMemberDetailsAdapter
    {
        private readonly HttpClient _httpClient;
        private APICredential apiCredential;

        public MemberDetailsAdapter(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            apiCredential = new APICredential(configuration);
        }
        public async Task<string> IsVisitedAsync(int memberId, int eventId, int loggedInUserId)
        {
            var url = $"{apiCredential.url}Member/IsVisited?memberid={memberId}&EventId={eventId}&loggedInUserId={loggedInUserId}";
            var response = await _httpClient.PostAsync(url, null);

            var json = await response.Content.ReadAsStringAsync();
            return json; 
        }

    }
}
