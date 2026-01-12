using System.Text.Json;
using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var apiKey = configuration.GetValue<string>("Authentication:ApiKey");
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            }
        }

        public async Task DeleteRegionAsync(int id)
        {
            var context = _httpContextAccessor.HttpContext;
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/region/{id}");

            if (context != null)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    requestMessage.Headers.Add("X-Audit-User", context.User.Identity.Name);
                }
                requestMessage.Headers.Add("X-Audit-IP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            
            if (!response.IsSuccessStatusCode)
            {
                 var errorContent = await response.Content.ReadAsStringAsync();
                 try 
                 {
                     var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                     if(errorObj.TryGetProperty("detail", out var detail)) 
                     {
                         throw new HttpRequestException(detail.GetString());
                     }
                 } 
                 catch (JsonException) { /* Fallback to raw content if not JSON */ }

                 throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }
        }

        public async Task DeleteComunaAsync(int regionId, int comunaId)
        {
            var context = _httpContextAccessor.HttpContext;
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/region/{regionId}/comuna/{comunaId}");

            if (context != null)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    requestMessage.Headers.Add("X-Audit-User", context.User.Identity.Name);
                }
                requestMessage.Headers.Add("X-Audit-IP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            
            if (!response.IsSuccessStatusCode)
            {
                 var errorContent = await response.Content.ReadAsStringAsync();
                 try 
                 {
                     var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                     if(errorObj.TryGetProperty("detail", out var detail)) 
                     {
                         throw new HttpRequestException(detail.GetString());
                     }
                 } 
                 catch (JsonException) { /* Fallback to raw content if not JSON */ }

                 throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");

            }
        }

        public async Task CreateRegionAsync(CreateRegionRequest request)
        {
            await SendPostAsync("/api/region", request);
        }

        public async Task CreateComunaAsync(CreateComunaRequest request)
        {
            await SendPostAsync($"/api/region/{request.IdRegion}/comuna", request);
        }

        private async Task SendPostAsync<T>(string uri, T payload)
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    content.Headers.Add("X-Audit-User", context.User.Identity.Name);
                }
                content.Headers.Add("X-Audit-IP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            }

            var response = await _httpClient.PostAsync(uri, content);
            
            if (!response.IsSuccessStatusCode)
            {
                 var errorContent = await response.Content.ReadAsStringAsync();
                 try 
                 {
                     var errorObj = JsonSerializer.Deserialize<JsonElement>(errorContent);
                     if(errorObj.TryGetProperty("detail", out var detail)) 
                     {
                         throw new HttpRequestException(detail.GetString());
                     }
                 } 
                 catch (JsonException) { /* Fallback to raw content if not JSON */ }

                 throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }
        }


        public async Task UpdateComunaAsync(int regionId, UpdateComunaRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    content.Headers.Add("X-Audit-User", context.User.Identity.Name);
                }
                content.Headers.Add("X-Audit-IP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            }
            
            var response = await _httpClient.PutAsync($"/api/region/{regionId}/comuna/{request.IdComuna}", content);
            
            if (!response.IsSuccessStatusCode)
            {
                 var errorContent = await response.Content.ReadAsStringAsync();
                 throw new HttpRequestException($"Error: {response.StatusCode} - {errorContent}");
            }
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



        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                content.Headers.Add("X-Audit-IP", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
            }

            var response = await _httpClient.PostAsync("/api/auth/login", content);
            
            return response.IsSuccessStatusCode;
        }
        public async Task<IEnumerable<Data.Models.Auditoria>> GetAuditLogsAsync()
        {
            var response = await _httpClient.GetAsync("/api/auditoria");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Data.Models.Auditoria>>(content, _options) ?? new List<Data.Models.Auditoria>();
        }
    }
}
