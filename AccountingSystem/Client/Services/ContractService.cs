using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;

namespace AccountingSystem.Client.Services
{
    public interface IContractService
    {
        Task<bool> CreateContract(Contract entity);
        Task<bool> UpdateContract(Contract entity);
        Task<List<Contract>> GetContractsAsync();
        Task<Contract> GetDetails(int id);
        Task<string> GetTotalPaidSoFar(int contractid);
    }

    public class ContractService : IContractService
    {
        public readonly HttpClient _http;
        public ContractService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            return await ApiWrapper.Get<List<Contract>>($"{_http.BaseAddress.AbsoluteUri}api/contract/getallcontracts");
        }

        public async Task<bool> CreateContract(Contract entity)
        {
            var result = await ApiWrapper.Post<bool>($"{_http.BaseAddress.AbsoluteUri}api/contract/createcontract", entity);
            return result;
        }

        public async Task<bool> UpdateContract(Contract entity)
        {
            var result = await ApiWrapper.Put<Contract>($"{_http.BaseAddress.AbsoluteUri}api/contract/updatecontract", entity);
            return result != null;
        }

        public async Task<Contract> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Contract>($"{_http.BaseAddress.AbsoluteUri}api/contract/getdetails/{id}");
            return res;
        }

        public async Task<string> GetTotalPaidSoFar(int contractid)
        {
            var res = await _http.GetStringAsync($"api/contract/GetTotalPaidSoFar/{contractid}");
            return res;
        }
    }
}
