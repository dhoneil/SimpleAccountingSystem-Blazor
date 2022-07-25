using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class PartNumberController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public PartNumberController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/PartNumber/getPartNumbers")]
        public async Task<List<PartNumber>> GetPartNumbersAsync()
        {
            var res = await _context.PartNumbers.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/PartNumber/create")]
        public async Task<bool> Create([FromBody] PartNumber entity)
        {
            await _context.PartNumbers.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/PartNumber/getdetails/{id}")]
        public async Task<PartNumber> GetDetails(int id)
        {
            var res = await _context.PartNumbers.FirstOrDefaultAsync(s => s.PartNumberId == id);
            return res;
        }

        [HttpPut, Route("api/PartNumber/update")]
        public async Task<IActionResult> Update(PartNumber entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.PartNumbers.FirstOrDefaultAsync(x => x.PartNumberId == entity.PartNumberId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
