using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;

namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {

        [HttpGet, Route("api/data/getall/{sql}/{connectionString}")]
        public async Task<List<T>> getall<T, U>(string sql, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql);
                return rows.ToList();
            }
        }

        [HttpGet, Route("api/data/create")]
        public Task Create<T>(string sql, T parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                return connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
