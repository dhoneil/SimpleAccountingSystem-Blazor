using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ItemController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Item/getItems")]
        public async Task<List<Item>> GetItemsAsync()
        {
            var res = await _context.Items.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Item/create")]
        public async Task<bool> Create([FromBody] Item entity)
        {
            await _context.Items.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Item/getdetails/{id}")]
        public async Task<Item> GetDetails(int id)
        {
            var res = await _context.Items.FirstOrDefaultAsync(s => s.ItemId == id);
            return res;
        }

        [HttpPut, Route("api/Item/update")]
        public async Task<IActionResult> Update(Item entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == entity.ItemId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
