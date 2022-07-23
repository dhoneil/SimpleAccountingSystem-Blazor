using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public PaymentTransactionController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/PaymentTransaction/getallPaymentTransactions")]
        public async Task<List<PaymentTransaction>> GetPaymentTransactionsAsync()
        {
            var res = await _context.PaymentTransactions.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/PaymentTransaction/createPaymentTransaction")]
        public async Task<bool> CreatePaymentTransaction([FromBody] PaymentTransaction entity)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest("Invalid data.");

            //await _context.PaymentTransactions.AddAsync(entity);
            //var res = await _context.SaveChangesAsync();

            //if (res != 1)
            //    return BadRequest("Failed during inserting to database");

            //return Ok(res);
            await _context.PaymentTransactions.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpPut, Route("api/PaymentTransaction/updatePaymentTransaction")]
        public async Task<IActionResult> UpdatePaymentTransaction(PaymentTransaction entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
