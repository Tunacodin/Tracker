using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Tracker.Api
{
    public class ApiService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<GenericResponse<T>> GetAsync<T>(string url)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    httpContextAccessor.HttpContext.Response.Redirect("/Auth/Index");
                    await httpContextAccessor.HttpContext.Response.CompleteAsync();
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new GenericResponse<T>
                    {
                        IsSuccess = false,
                        Message =
                            $"API request failed with status {response.StatusCode}. Response: {errorContent}",
                    };
                }

                return await response.Content.ReadFromJsonAsync<GenericResponse<T>>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (JsonException ex)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = "Failed to deserialize the API response. " + ex.Message,
                };
            }
        }

        public async Task<GenericResponse<T>> PostAsync<T>(string url, object data)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await httpClient.PostAsJsonAsync(url, data);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    httpContextAccessor.HttpContext.Response.Redirect("/Auth/Index");
                    await httpContextAccessor.HttpContext.Response.CompleteAsync();
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new GenericResponse<T>
                    {
                        IsSuccess = false,
                        Message =
                            $"API request failed with status {response.StatusCode}. Response: {errorContent}",
                    };
                }

                return await response.Content.ReadFromJsonAsync<GenericResponse<T>>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (JsonException)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = "Failed to deserialize the API response.",
                };
            }
        }

        public async Task<GenericResponse<T>> DeleteAsync<T>(string url, object? data)
        {
            try
            {
                AddAuthorizationHeader();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                if (data != null)
                {
                    request.Content = JsonContent.Create(data);
                }

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    httpContextAccessor.HttpContext.Response.Redirect("/Auth/Index");
                    await httpContextAccessor.HttpContext.Response.CompleteAsync();
                    return default;
                }
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new GenericResponse<T>
                    {
                        IsSuccess = false,
                        Message =
                            $"API request failed with status {response.StatusCode}. Response: {errorContent}",
                    };
                }

                return await response.Content.ReadFromJsonAsync<GenericResponse<T>>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (JsonException)
            {
                return new GenericResponse<T>
                {
                    IsSuccess = false,
                    Message = "Failed to deserialize the API response.",
                };
            }
        }

        private void AddAuthorizationHeader()
        {
            //var token = httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
            var tokenFromClaim = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(tokenFromClaim))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    tokenFromClaim
                );
            }
        }
    }
}
