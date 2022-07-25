using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrands();
        Task<bool> CreateItem(Brand entity);
        Task<Brand> GetDetails(int id);
        Task<bool> UpdateItem(Brand entity);
    }

    public class BrandService : IBrandService
    {
        public readonly HttpClient _http;
        public BrandService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Brand>> GetBrands()
        {
            return await ApiWrapper.Get<List<Brand>>($"{_http.BaseAddress.AbsoluteUri}api/Brand/getBrands");
        }

        public async Task<bool> CreateItem(Brand entity)
        {
            var result = await _http.PostAsJsonAsync("api/Brand/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Brand> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Brand>($"{_http.BaseAddress.AbsoluteUri}api/Brand/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Brand entity)
        {
            var result = await ApiWrapper.Put<Brand>($"{_http.BaseAddress.AbsoluteUri}api/Brand/update", entity);
            return result != null;
        }
    }
}
