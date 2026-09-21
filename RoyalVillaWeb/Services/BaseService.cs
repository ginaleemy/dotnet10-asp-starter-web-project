using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        // ApiResponse<object> IBaseService.ResponseModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IHttpClientFactory _httpClient { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiResponse<object> ResponseModel { get; set; }
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.ResponseModel = new();
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {

            try
            {
                var client = _httpClient.CreateClient("RoyalVillaAPI");
                var message = new HttpRequestMessage
                {
                    // Fix 1: Support both Absolute and Relative URIs safely
                    // RequestUri = new Uri(apiRequest.Url, uriKind: UriKind.Relative),
                    // Method = GetHttpMethod(apiRequest.ApiType)
                    RequestUri = new Uri(apiRequest.Url, UriKind.RelativeOrAbsolute),
                    Method = GetHttpMethod(apiRequest.ApiType)
                };
                // Fix 2: Attach JWT Bearer Token if user is logged in
                var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken"); // Replace with your SD.SessionToken key
                if (!string.IsNullOrEmpty(token))
                {
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                if (apiRequest.Data != null)
                {
                    message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions);
                }
                var apiResponse = await client.SendAsync(message);
                // Read response safely
                if (!apiResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Request failed with status code: {apiResponse.StatusCode}");
                }

                return await apiResponse.Content.ReadFromJsonAsync<T>(JsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return default;
            }
        }
        private static HttpMethod GetHttpMethod(SD.ApiType apiType)
        {
            return apiType switch
            {
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }

    }
}
