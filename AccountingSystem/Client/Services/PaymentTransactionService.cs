using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;


namespace AccountingSystem.Client.Services
{
    public interface IPaymentTransactionService
    {
        Task<bool> CreatePaymentTransaction(PaymentTransaction entity);
        Task<List<PaymentTransaction>> GetPaymentTransactionsAsync();
        Task<bool> UpdatePaymentTransaction(PaymentTransaction entity);
    }

    public class PaymentTransactionService : IPaymentTransactionService
    {
        public readonly HttpClient _http;
        public PaymentTransactionService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactionsAsync()
        {
            return await ApiWrapper.Get<List<PaymentTransaction>>($"{_http.BaseAddress.AbsoluteUri}api/PaymentTransaction/getallPaymentTransactions");
        }

        public async Task<bool> CreatePaymentTransaction(PaymentTransaction entity)
        {
            var result = await ApiWrapper.Post<bool>($"{_http.BaseAddress.AbsoluteUri}api/PaymentTransaction/createPaymentTransaction", entity);
            return result;
        }

        public async Task<bool> UpdatePaymentTransaction(PaymentTransaction entity)
        {
            var result = await ApiWrapper.Put<PaymentTransaction>($"{_http.BaseAddress.AbsoluteUri}api/PaymentTransaction/updatePaymentTransaction", entity);
            return result != null;
        }
    }
}
