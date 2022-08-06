using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers();
        Task<bool> CreateItem(Customer entity);
        Task<Customer> GetDetails(int id);
        Task<bool> UpdateItem(Customer entity);
    }

    public class CustomerService : ICustomerService
    {
        public readonly HttpClient _http;
        public CustomerService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await ApiWrapper.Get<List<Customer>>($"{_http.BaseAddress.AbsoluteUri}api/Customer/getCustomers");
        }

        public async Task<bool> CreateItem(Customer entity)
        {
            var result = await _http.PostAsJsonAsync("api/Customer/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Customer> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Customer>($"{_http.BaseAddress.AbsoluteUri}api/Customer/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateItem(Customer entity)
        {
            var result = await ApiWrapper.Put<Customer>($"{_http.BaseAddress.AbsoluteUri}api/Customer/update", entity);
            return result != null;
        }
    }
}
