using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ExpenseController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Expense/getallExpenses")]
        public async Task<List<Expense>> GetExpensesAsync()
        {
            var res = await _context.Expenses.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Expense/createExpense")]
        public async Task<bool> CreateExpense([FromBody] Expense entity)
        {
            await _context.Expenses.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpPut, Route("api/Expense/updateExpense")]
        public async Task<IActionResult> UpdateExpense(Expense entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
