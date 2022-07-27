using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AccountingSystem.Shared.GlobalVariables;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class HelperController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public HelperController(accounting_systemContext context)
        {
            _context = context;
        }

        public static async Task<string> DBqueryAsync(string query)
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

        [HttpGet, Route("api/Helper/GetLastId/{tablename}/{tablepk}")]
        public async Task<string> GetLastId(string tablename,string tablepk)
        {
            var query = @$"SELECT TOP 1 {tablepk} FROM {tablename} ORDER BY {tablepk} DESC";
            var res = await DBqueryAsync(query);
            return res;
        }
    }
}
