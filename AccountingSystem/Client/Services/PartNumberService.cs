using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IPartNumberService
    {
        Task<List<PartNumber>> GetPartNumbers();
        Task<bool> CreateItem(PartNumber entity);
        Task<PartNumber> GetDetails(int id);
        Task<bool> UpdateItem(PartNumber entity);
    }

    public class PartNumberService : IPartNumberService
    {
        public readonly HttpClient _http;
        public PartNumberService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<PartNumber>> GetPartNumbers()
        {
            return await ApiWrapper.Get<List<PartNumber>>($"{_http.BaseAddress.AbsoluteUri}api/PartNumber/getPartNumbers");
        }

        public async Task<bool> CreateItem(PartNumber entity)
        {
            var result = await _http.PostAsJsonAsync("api/PartNumber/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<PartNumber> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<PartNumber>($"{_http.BaseAddress.AbsoluteUri}api/PartNumber/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(PartNumber entity)
        {
            var result = await ApiWrapper.Put<PartNumber>($"{_http.BaseAddress.AbsoluteUri}api/PartNumber/update", entity);
            return result != null;
        }
    }
}
