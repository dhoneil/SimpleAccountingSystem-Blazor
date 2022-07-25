using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IUomService
    {
        Task<List<Uom>> GetUoms();
        Task<bool> CreateItem(Uom entity);
        Task<Uom> GetDetails(int id);
        Task<bool> UpdateItem(Uom entity);
    }

    public class UomService : IUomService
    {
        public readonly HttpClient _http;
        public UomService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Uom>> GetUoms()
        {
            return await ApiWrapper.Get<List<Uom>>($"{_http.BaseAddress.AbsoluteUri}api/Uom/getUoms");
        }

        public async Task<bool> CreateItem(Uom entity)
        {
            var result = await _http.PostAsJsonAsync("api/Uom/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Uom> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Uom>($"{_http.BaseAddress.AbsoluteUri}api/Uom/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Uom entity)
        {
            var result = await ApiWrapper.Put<Uom>($"{_http.BaseAddress.AbsoluteUri}api/Uom/update", entity);
            return result != null;
        }
    }
}
