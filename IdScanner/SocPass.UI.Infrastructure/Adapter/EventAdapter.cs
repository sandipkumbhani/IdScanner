using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Interfaces;
using System.Net.Http;
using System.Text;

namespace SocPass.UI.Infrastructure.Provider
{
    public class EventAdapter : IEventAdapter
    {
        private readonly ICommonAdapter _commonAdapter;

        public EventAdapter(ICommonAdapter commonAdapter)
        {
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<Event>> GetAllEventAsync()
        {
            return await _commonAdapter.GetAsync<IList<Event>>("Event/getAllEvents");
        }
        public async Task<IList<Event>> GetEventBySocietyId(int SocietyId)
        {
            return await _commonAdapter.GetAsync<IList<Event>>($"Event/getEventBySocietyId?SocietyId={SocietyId}");
        }
        public async Task<IList<Event>> GetEventByUserId(int userid)
        {
            return await _commonAdapter.GetAsync<IList<Event>>($"Event/getEventByUserId?userid={userid}");
        }
        public async Task<string> AddEventAsync(Event events)
        {
            try
            {
                var response = await _commonAdapter.PostAsync<CommanResponseDto<Event>>("Event/Add-Event", events);

                return response?.Message ?? "Event added successfully.";
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

        public async Task<Event> GetEventByIdAsync(int? eventId)
        {
            return await _commonAdapter.GetAsync<Event>($"Event/GetEventById?eventId={eventId}");
        }
        public async Task<string> UpdateEventAsync(Event events)
        {
            return await _commonAdapter.PutAsync($"Event/Update-Event", events);
        }
        public async Task<string> DeleteMenuAsync(int eventId)
        {
            return await _commonAdapter.DeleteAsync<string>($"Event/Delete-Event?eventId={eventId}");
        }

    }
}
