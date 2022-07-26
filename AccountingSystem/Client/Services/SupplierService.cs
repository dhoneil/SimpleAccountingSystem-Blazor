using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetSuppliers();
        Task<bool> CreateItem(Supplier entity);
        Task<Supplier> GetDetails(int id);
        Task<bool> UpdateItem(Supplier entity);
    }

    public class SupplierService : ISupplierService
    {
        public readonly HttpClient _http;
        public SupplierService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Supplier>> GetSuppliers()
        {
            return await ApiWrapper.Get<List<Supplier>>($"{_http.BaseAddress.AbsoluteUri}api/Supplier/getSuppliers");
        }

        public async Task<bool> CreateItem(Supplier entity)
        {
            var result = await _http.PostAsJsonAsync("api/Supplier/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Supplier> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Supplier>($"{_http.BaseAddress.AbsoluteUri}api/Supplier/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Supplier entity)
        {
            var result = await ApiWrapper.Put<Supplier>($"{_http.BaseAddress.AbsoluteUri}api/Supplier/update", entity);
            return result != null;
        }
    }
}
