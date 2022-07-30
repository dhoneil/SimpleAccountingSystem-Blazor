using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IReceivedItemService
    {
        Task<List<ReceivedItem>> GetReceivedItems();
        Task<bool> CreateItem(ReceivedItem entity);
        Task<ReceivedItem> GetDetails(int id);
        Task<bool> UpdateItem(ReceivedItem entity);
        Task<ReceivedItem> AddNewReceiveItem(ReceivedItem entity);
        Task<bool> AddNewReceiveItemDetail(ReceivedItemDetail entity);
        Task<ReceivedItem> GetLastReceiveItem();
    }

    public class ReceivedItemService : IReceivedItemService
    {
        public readonly HttpClient _http;
        public ReceivedItemService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<ReceivedItem>> GetReceivedItems()
        {
            return await ApiWrapper.Get<List<ReceivedItem>>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/getReceivedItems");
        }

        public async Task<bool> CreateItem(ReceivedItem entity)
        {
            var result = await _http.PostAsJsonAsync("api/ReceivedItem/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<ReceivedItem> AddNewReceiveItem(ReceivedItem entity)
        {
            var res = await ApiWrapper.Post<ReceivedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/AddNewReceiveItem",entity);
            return res;
        }

        public async Task<bool> AddNewReceiveItemDetail(ReceivedItemDetail entity)
        {
            //var res = await ApiWrapper.Post<ReceivedItemDetail>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/AddNewReceiveItemDetail", entity);
            //return res;
            var result = await _http.PostAsJsonAsync("api/ReceivedItem/AddNewReceiveItemDetail", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<ReceivedItem> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<ReceivedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(ReceivedItem entity)
        {
            var result = await ApiWrapper.Put<ReceivedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/update", entity);
            return result != null;
        }

        public async Task<ReceivedItem> GetLastReceiveItem()
        {
            var res = await ApiWrapper.Get<ReceivedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReceivedItem/getLastReceiveItem");
            return res;
        }
    }
}
