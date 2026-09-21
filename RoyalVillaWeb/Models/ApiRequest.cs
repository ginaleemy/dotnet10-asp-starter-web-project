using static RoyalVillaWeb.SD;

namespace RoyalVillaWeb.Models
{
    //Added Manual
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string? Url { get; set; }
        public object? Data { get; set; }
        public string? Token { get; set; }
    }
}
