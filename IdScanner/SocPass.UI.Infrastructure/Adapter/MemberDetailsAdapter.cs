using Microsoft.Extensions.Configuration;
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
    public class MemberDetailsAdapter : IMemberDetailsAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public MemberDetailsAdapter(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
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
