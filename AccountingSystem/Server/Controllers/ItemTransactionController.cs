using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ItemTransactionController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ItemTransactionController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/ItemTransaction/getItemTransactions")]
        public async Task<List<ItemTransaction>> GetItemTransactionsAsync()
        {
            var res = await _context.ItemTransactions.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/ItemTransaction/create")]
        public async Task<bool> Create([FromBody] ItemTransaction entity)
        {
            await _context.ItemTransactions.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpPost, Route("api/ItemTransaction/createMany")]
        public async Task<bool> CreateMany([FromBody] List<ItemTransaction> entity)
        {
            await _context.ItemTransactions.AddRangeAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/ItemTransaction/getdetails/{id}")]
        public async Task<ItemTransaction> GetDetails(int id)
        {
            var res = await _context.ItemTransactions.FirstOrDefaultAsync(s => s.ItemTransactionId == id);
            return res;
        }

        [HttpPut, Route("api/ItemTransaction/update")]
        public async Task<IActionResult> Update(ItemTransaction entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.ItemTransactions.FirstOrDefaultAsync(x => x.ItemTransactionId == entity.ItemTransactionId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
