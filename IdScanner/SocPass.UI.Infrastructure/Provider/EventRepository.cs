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
    public class EventRepository : IEventRepository
    {
        private readonly HttpClient _httpClinet;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private GlobalClass _globalClass;

        public EventRepository(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClinet = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<List<Event>> GetAllEventAsync()
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "Event/getAllEvents";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Event>>(jsonString)!;
        }
        public async Task<List<Event>> GetEventBySocietyId(int SocietyId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Event/getEventBySocietyId?SocietyId={SocietyId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Event>>(jsonString)!;
        }
        public async Task<string> AddEventAsync(Event events)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + "Event/Add-Event";
            var userJson = JsonConvert.SerializeObject(events);
            var requestContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await _httpClinet.PostAsync(baseUrl, requestContent);
            var responseData = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                string message;

                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                    message = errorResponse?.ErrorMessage
                        ?? errorResponse?.Message
                        ?? "Failed to create Event.";
                }
                catch (JsonReaderException)
                {
                    message = responseData;
                }

                throw new Exception($"API Error ({response.StatusCode}): {message}");
            }
            return "Event added successfully.";
        }
        public async Task<Event> GetEventByIdAsync(int? eventId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Event/GetEventById?eventId={eventId}";
            var response = await _httpClinet.GetAsync(baseUrl);
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Event>(jsonString)!;
        }
        public async Task<string> UpdateEventAsync(Event events)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Event/Update-Event/{events.EventId}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(events), Encoding.UTF8, "application/json");
            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> DeleteMenuAsync(int eventId)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = apiCredential.url + $"Event/Delete-Event?eventId={eventId}";
            var response = await _httpClinet.DeleteAsync(baseUrl);
            return await response.Content.ReadAsStringAsync();
        }


    }
}
