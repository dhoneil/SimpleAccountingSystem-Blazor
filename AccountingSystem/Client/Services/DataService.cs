using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using AccountingSystem.Shared.Utility;
using AccountingSystem.Shared.ViewModels.MySql;

namespace AccountingSystem.Client.Services
{
    public interface IDataService
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString);
        Task<List<T>> GetAll<T>(string sql, string connectionString);
    }

    public class DataService : IDataService
    {
        public readonly HttpClient _http;
        public DataService(HttpClient httpClient)
        {
            _http = httpClient;
        }
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }

        public async Task<List<T>> GetAll<T>(string sql, string connectionString)
        {
            var result = await ApiWrapper.Get<List<T>>($"{_http.BaseAddress.AbsoluteUri}api/data/getall/{sql}/{connectionString}");
            //var res = await _http.GetAsync<Customer,dynamic>($"api/data/getall/{sql}/{connectionString}");
            return null;
        }
    }
}
