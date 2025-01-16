using Application.Utilities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string url);
        Task<JsonResponse<T>> PostAsync<T>(string url, object data);
        Task<JsonResponse<T>> PostAsync<T>(string url, object data,string token);
    }
}
