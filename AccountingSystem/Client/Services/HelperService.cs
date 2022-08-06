using System.Data;
using AccountingSystem.Shared.GlobalVariables;
using AccountingSystem.Shared.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AccountingSystem.Client.Services
{
    public interface IHelperService
    {
        Task<string> DBqueryAsync(string query);
        string DBquery(string query);
        Task<string> GetLastId(string tablename, string tablepk);
        Task ModalAction(string modalid, string action);
        string GenerateTransactionNo(int length);
    }

    public class HelperService : IHelperService
    {
        public readonly IJSRuntime _JS;
        public readonly HttpClient _http;
        public HelperService(IJSRuntime JS, HttpClient http)
        {
            _JS = JS;
            _http = http;
        }

        public async Task<string> DBqueryAsync(string query)
        {
            var result = string.Empty;
            DataTable dt = new DataTable();
            var connstring = GlobalVariables.CONNECTIONSTRING;
            using (SqlConnection con = new SqlConnection(connstring))
            {
                SqlCommand objSqlCommand = new SqlCommand(query, con);
                objSqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                try
                {
                    objSqlDataAdapter.Fill(dt);
                    string jsonresult = JsonConvert.SerializeObject(dt);
                    result = jsonresult;
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
                finally
                {
                    await objSqlCommand.DisposeAsync();
                    await con.CloseAsync();
                }
            }
            await Task.CompletedTask;
            return result;
        }

        public string DBquery(string query)
        {
            var result = string.Empty;
            DataTable dt = new DataTable();
            var connstring = GlobalVariables.CONNECTIONSTRING;
            using (SqlConnection con = new SqlConnection(connstring))
            {
                SqlCommand objSqlCommand = new SqlCommand(query, con);
                objSqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand);
                try
                {
                    objSqlDataAdapter.Fill(dt);
                    string jsonresult = JsonConvert.SerializeObject(dt);
                    result = jsonresult;
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
                finally
                {
                    objSqlCommand.DisposeAsync();
                    con.CloseAsync();
                }
            }
            return result;
        }

        public async Task ModalAction(string modalid, string action)
        {
            await _JS.InvokeVoidAsync("Select2Trigger", modalid);
            await _JS.InvokeVoidAsync("ModalAction", modalid, action);
        }

        public async Task<string> GetLastId(string tablename,string tablepk)
        {
            //var res = ApiWrapper.Get<int>($"{_http.BaseAddress.AbsoluteUri}api/Helper/GetLastId/{tablename}");
            var res = await _http.GetStringAsync($"api/helper/getlastid/{tablename}/{tablepk}");
            return res;
        }

        public string GenerateTransactionNo(int length)
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var res = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
            return res;
        }
    }
}
