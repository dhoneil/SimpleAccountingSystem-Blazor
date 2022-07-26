using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
        Task<bool> CreateItem(Category entity);
        Task<Category> GetDetails(int id);
        Task<bool> UpdateItem(Category entity);
    }

    public class CategoryService : ICategoryService
    {
        public readonly HttpClient _http;
        public CategoryService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await ApiWrapper.Get<List<Category>>($"{_http.BaseAddress.AbsoluteUri}api/Category/getCategories");
        }

        public async Task<bool> CreateItem(Category entity)
        {
            var result = await _http.PostAsJsonAsync("api/Category/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Category> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Category>($"{_http.BaseAddress.AbsoluteUri}api/Category/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Category entity)
        {
            var result = await ApiWrapper.Put<Category>($"{_http.BaseAddress.AbsoluteUri}api/Category/update", entity);
            return result != null;
        }
    }
}
