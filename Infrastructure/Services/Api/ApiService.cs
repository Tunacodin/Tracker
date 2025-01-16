using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Services;
using Application.Utilities.Response;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Api
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result);
        }

        // public async Task<JsonResponse<T>> PostAsync<T>(string url, object data)
        // {
        //     var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        //     content.Headers.Add("whitekey", "70ebba4a-f651-417c-b104-b9de18345f19");
        //     var response = await _httpClient.PostAsync(url, content);
        //     response.EnsureSuccessStatusCode();
        //     var result = await response.Content.ReadAsStringAsync();
        //     var jsonData = JsonSerializer.Deserialize<JsonResponse<T>>(result);
        //     return jsonData;
        // }
        // public async Task<JsonResponse<T>> PostAsync<T>(string url, object data, string token)
        // {
        //     var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        //     using var request = new HttpRequestMessage(HttpMethod.Post, url)
        //     {
        //         Content = content
        //     };
        //     request.Headers.Add("whitekey", "70ebba4a-f651-417c-b104-b9de18345f19");
        //     try
        //     {
        //         request.Headers.TryAddWithoutValidation("Authorization", token);
        //         using var response = await _httpClient.SendAsync(request);
        //         //response.EnsureSuccessStatusCode();
        //         var result = await response.Content.ReadAsStringAsync();
        //         var jsonData = JsonSerializer.Deserialize<JsonResponse<T>>(result);
        //         return jsonData;
        //     }
        //     catch (Exception ex)
        //     {

        //         throw;
        //     }

        // }

        public async Task<JsonResponse<T>> PostAsync<T>(string url, object data)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                );
                content.Headers.Add("whitekey", _configuration["ApiSettings:WhiteKey"]);

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return default;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<JsonResponse<T>>(result);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<JsonResponse<T>> PostAsync<T>(string url, object data, string token)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                );

                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content,
                };

                request.Headers.Add("whitekey", _configuration["ApiSettings:WhiteKey"]);
                request.Headers.TryAddWithoutValidation("Authorization", token);

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return default;
                }

                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<JsonResponse<T>>(result);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
