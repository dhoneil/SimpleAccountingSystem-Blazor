using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IReleasedItemService
    {
        Task<List<ReleasedItem>> GetReleasedItems();
        Task<bool> CreateItem(ReleasedItem entity);
        Task<ReleasedItem> GetDetails(int id);
        Task<bool> UpdateItem(ReleasedItem entity);
        Task<ReleasedItem> AddNewReleaseItem(ReleasedItem entity);
        Task<bool> AddNewReleaseItemDetails(List<ReleasedItemDetail> entity);
        Task<ReleasedItem> GetLastReleaseItem();
        Task<List<ReleasedItemDetail>> GetReleasedItemDetailsAsync(int Releaseditemid);
    }

    public class ReleasedItemService : IReleasedItemService
    {
        public readonly HttpClient _http;
        public ReleasedItemService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<ReleasedItem>> GetReleasedItems()
        {
            return await ApiWrapper.Get<List<ReleasedItem>>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/getReleasedItems");
        }

        public async Task<bool> CreateItem(ReleasedItem entity)
        {
            var result = await _http.PostAsJsonAsync("api/ReleasedItem/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<ReleasedItem> AddNewReleaseItem(ReleasedItem entity)
        {
            var res = await ApiWrapper.Post<ReleasedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/AddNewReleaseItem",entity);
            return res;
        }

        public async Task<bool> AddNewReleaseItemDetails(List<ReleasedItemDetail> entity)
        {
            //var res = await ApiWrapper.Post<ReleasedItemDetail>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/AddNewReleaseItemDetail", entity);
            //return res;
            var result = await _http.PostAsJsonAsync("api/ReleasedItem/AddNewReleaseItemDetails", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<ReleasedItem> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<ReleasedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(ReleasedItem entity)
        {
            var result = await ApiWrapper.Put<ReleasedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/update", entity);
            return result != null;
        }

        public async Task<ReleasedItem> GetLastReleaseItem()
        {
            var res = await ApiWrapper.Get<ReleasedItem>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/getLastReleaseItem");
            return res;
        }

        public async Task<List<ReleasedItemDetail>> GetReleasedItemDetailsAsync(int Releaseditemid)
        {
            var res = await ApiWrapper.Get<List<ReleasedItemDetail>>($"{_http.BaseAddress.AbsoluteUri}api/ReleasedItem/getReleasedItemDetails/{Releaseditemid}");
            return res;
        }
    }
}
