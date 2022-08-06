using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IItemTransactionService
    {
        Task<List<ItemTransaction>> GetItemTransactions();
        Task<bool> CreateItem(ItemTransaction entity);
        Task<bool> CreateManyItem(List<ItemTransaction> entity);
        Task<ItemTransaction> GetDetails(int id);
        Task<bool> UpdateItem(ItemTransaction entity);
    }

    public class ItemTransactionService : IItemTransactionService
    {
        public readonly HttpClient _http;
        public ItemTransactionService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<ItemTransaction>> GetItemTransactions()
        {
            return await ApiWrapper.Get<List<ItemTransaction>>($"{_http.BaseAddress.AbsoluteUri}api/ItemTransaction/getItemTransactions");
        }

        public async Task<bool> CreateItem(ItemTransaction entity)
        {
            var result = await _http.PostAsJsonAsync("api/ItemTransaction/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> CreateManyItem(List<ItemTransaction> entity)
        {
            var result = await _http.PostAsJsonAsync("api/ItemTransaction/createMany", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<ItemTransaction> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<ItemTransaction>($"{_http.BaseAddress.AbsoluteUri}api/ItemTransaction/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(ItemTransaction entity)
        {
            var result = await ApiWrapper.Put<ItemTransaction>($"{_http.BaseAddress.AbsoluteUri}api/ItemTransaction/update", entity);
            return result != null;
        }
    }
}
