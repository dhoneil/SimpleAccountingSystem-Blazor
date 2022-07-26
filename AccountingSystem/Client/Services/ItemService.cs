using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IItemService
    {
        Task<List<Item>> GetItems();
        Task<bool> CreateItem(Item entity);
        Task<Item> GetDetails(int id);
        Task<bool> UpdateItem(Item entity);
    }

    public class ItemService : IItemService
    {
        public readonly HttpClient _http;
        public ItemService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Item>> GetItems()
        {
            return await ApiWrapper.Get<List<Item>>($"{_http.BaseAddress.AbsoluteUri}api/Item/getItems");
        }

        public async Task<bool> CreateItem(Item entity)
        {
            var result = await _http.PostAsJsonAsync("api/Item/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Item> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Item>($"{_http.BaseAddress.AbsoluteUri}api/Item/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Item entity)
        {
            var result = await ApiWrapper.Put<Item>($"{_http.BaseAddress.AbsoluteUri}api/Item/update", entity);
            return result != null;
        }
    }
}
