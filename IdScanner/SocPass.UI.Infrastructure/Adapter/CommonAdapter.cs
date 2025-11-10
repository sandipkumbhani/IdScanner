using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Comman;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System.Net.Http;
using System.Text;

namespace SocPass.UI.Infrastructure.Provider
{
    public class CommonAdapter : ICommonAdapter
    {
        private readonly HttpClient _httpClinet;
        private APICredential _apiCredential;
        private GlobalClass _globalClass;

        public CommonAdapter(HttpClient httpClinet, APICredential apiCredential, GlobalClass globalClass)
        {
            _httpClinet = httpClinet;
            _apiCredential = apiCredential;
            _globalClass = globalClass;
        }

        public async Task<T> GetAsync<T>(string EndPoint)
        {
            _httpClinet.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var baseUrl = _apiCredential.url + EndPoint;
            var response = await _httpClinet.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        //public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        //{
        //    _httpClinet.DefaultRequestHeaders.Authorization =
        //        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

        //    var baseUrl = _apiCredential.url + endpoint;

        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClinet.PostAsync(baseUrl, content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new HttpRequestException(
        //            $"API Error ({response.StatusCode}): {responseContent}",
        //            null,
        //            response.StatusCode);
        //    }

        //    if (typeof(TResponse) == typeof(string))
        //    {
        //        return (TResponse)(object)responseContent;
        //    }

        //    return JsonConvert.DeserializeObject<TResponse>(responseContent)!;
        //}
        public async Task<T> PostAsync<T>(string endPoint, object? data = null)
        {
            _httpClinet.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var url = _apiCredential.url + endPoint;

            StringContent? content = null;
            if (data != null)
            {
                var json = JsonConvert.SerializeObject(data);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            var response = await _httpClinet.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"API Error ({response.StatusCode}): {responseContent}",
                    null,
                    response.StatusCode);
            }
            return JsonConvert.DeserializeObject<T>(responseContent)!;
        }
        public async Task<string> PutAsync<TRequest>(string endpoint, TRequest data)
        {
            _httpClinet.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = _apiCredential.url + endpoint;
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await _httpClinet.PutAsync(baseUrl, jsonContent);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error ({response.StatusCode}): {responseData}");
            }

            return responseData;
        }
        public async Task<string> DeleteAsync<TResponse>(string endpoint)
        {
            _httpClinet.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = _apiCredential.url + endpoint;
            var response = await _httpClinet.DeleteAsync(baseUrl);
            var responseData = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error ({response.StatusCode}): {responseData}");
            }

            return responseData; 
        }
        public async Task<TResponse> addUpdateMemberAndGuestAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            _httpClinet.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var baseUrl = _apiCredential.url + endpoint;
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClinet.PutAsync(baseUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"API Error ({response.StatusCode}): {responseContent}",
                    null,
                    response.StatusCode);
            }

            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)responseContent;
            }

            return JsonConvert.DeserializeObject<TResponse>(responseContent)!;
        }




    }
}
