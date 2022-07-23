using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface IExpenseService
    {
        Task<bool> CreateExpense(Expense entity);
        Task<List<Expense>> GetExpensesAsync();
        Task<bool> UpdateExpense(Expense entity);
    }

    public class ExpenseService : IExpenseService
    {
        public readonly HttpClient _http;
        public ExpenseService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await ApiWrapper.Get<List<Expense>>($"{_http.BaseAddress.AbsoluteUri}api/Expense/getallExpenses");
        }

        public async Task<bool> CreateExpense(Expense entity)
        {
            //var result = await ApiWrapper.Post<bool>($"{_http.BaseAddress.AbsoluteUri}api/Expense/createExpense", entity);
            var result = await _http.PostAsJsonAsync("api/Expense/createExpense", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateExpense(Expense entity)
        {
            var result = await ApiWrapper.Put<Expense>($"{_http.BaseAddress.AbsoluteUri}api/Expense/updateExpense", entity);
            return result != null;
        }
    }
}
