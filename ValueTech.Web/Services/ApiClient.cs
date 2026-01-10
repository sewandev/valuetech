using System.Text.Json;
using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<RegionResponse>> GetRegionsAsync()
        {
            var response = await _httpClient.GetAsync("/api/region");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<RegionResponse>>(content, _options) ?? new List<RegionResponse>();
        }

        public async Task<IEnumerable<ComunaResponse>> GetComunasByRegionAsync(int regionId)
        {
            var response = await _httpClient.GetAsync($"/api/region/{regionId}/comuna");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ComunaResponse>>(content, _options) ?? new List<ComunaResponse>();
        }

        public async Task<ComunaResponse?> GetComunaByIdAsync(int regionId, int comunaId)
        {
            var response = await _httpClient.GetAsync($"/api/region/{regionId}/comuna/{comunaId}");
            if (!response.IsSuccessStatusCode) return null;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ComunaResponse>(content, _options);
        }

        public async Task UpdateComunaAsync(int regionId, CreateComunaRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"/api/region/{regionId}/comuna", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = JsonDocument.Parse(errorContent);
                    if (doc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        throw new HttpRequestException(errorElement.GetString());
                    }
                }
                catch (JsonException) { } 
                
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
