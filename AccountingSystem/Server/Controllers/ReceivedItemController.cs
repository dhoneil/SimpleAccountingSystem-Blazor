using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ReceivedItemController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ReceivedItemController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/ReceivedItem/getReceivedItems")]
        public async Task<List<ReceivedItem>> GetReceivedItemsAsync()
        {
            var res = await _context.ReceivedItems.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/ReceivedItem/create")]
        public async Task<bool> Create([FromBody] ReceivedItem entity)
        {
            await _context.ReceivedItems.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/ReceivedItem/getdetails/{id}")]
        public async Task<ReceivedItem> GetDetails(int id)
        {
            var res = await _context.ReceivedItems.FirstOrDefaultAsync(s => s.ReceivedItemId == id);
            return res;
        }

        [HttpPut, Route("api/ReceivedItem/update")]
        public async Task<IActionResult> Update(ReceivedItem entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.ReceivedItems.FirstOrDefaultAsync(x => x.ReceivedItemId == entity.ReceivedItemId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
