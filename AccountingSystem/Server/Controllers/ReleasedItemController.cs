using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ReleasedItemController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ReleasedItemController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/ReleasedItem/getReleasedItems")]
        public async Task<List<ReleasedItem>> GetReleasedItemsAsync()
        {
            var res = await _context.ReleasedItems.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/ReleasedItem/create")]
        public async Task<bool> Create([FromBody] ReleasedItem entity)
        {
            await _context.ReleasedItems.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpPost, Route("api/ReleasedItem/AddNewReceiveItem")]
        public async Task<IActionResult> AddNewReceiveItem([FromBody] ReleasedItem entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            await _context.ReleasedItems.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            if (res <= 0)
                return BadRequest("Failed during inserting to database");

            return Ok(res);
        }

        [HttpPost, Route("api/ReleasedItem/AddNewReleaseItemDetails")]
        public async Task<IActionResult> AddNewReceiveItemDetails([FromBody] List<ReleasedItemDetail> entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            await _context.ReleasedItemDetails.AddRangeAsync(entity);
            var res = await _context.SaveChangesAsync();
            if (res <= 0)
                return BadRequest("Failed during inserting to database");

            return Ok(res);
        }

        [HttpGet, Route("api/ReleasedItem/getdetails/{id}")]
        public async Task<ReleasedItem> GetDetails(int id)
        {
            var res = await _context.ReleasedItems.FirstOrDefaultAsync(s => s.ReleasedItemId == id);
            return res;
        }

        [HttpPut, Route("api/ReleasedItem/update")]
        public async Task<IActionResult> Update(ReleasedItem entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.ReleasedItems.FirstOrDefaultAsync(x => x.ReleasedItemId == entity.ReleasedItemId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }

        [HttpGet, Route("api/ReleasedItem/getLastReleaseItem")]
        public async Task<ReleasedItem> getLastReleaseItem()
        {
            var res = await _context.ReleasedItems.OrderByDescending(s => s.ReleasedItemId).FirstOrDefaultAsync();
            return res;
        }

        [HttpGet, Route("api/ReleasedItem/getReleasedItemDetails/{Releaseditemid}")]
        public async Task<List<ReleasedItemDetail>> GetReleasedItemDetailsAsync(int Releaseditemid)
        {
            var res = await _context.ReleasedItemDetails.Where(s=>s.ReleasedItemId == Releaseditemid).ToListAsync();
            return res;
        }
    }
}
