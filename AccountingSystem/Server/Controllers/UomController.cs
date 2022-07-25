using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class UomController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public UomController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Uom/getUoms")]
        public async Task<List<Uom>> GetUomsAsync()
        {
            var res = await _context.Uoms.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Uom/create")]
        public async Task<bool> Create([FromBody] Uom entity)
        {
            await _context.Uoms.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Uom/getdetails/{id}")]
        public async Task<Uom> GetDetails(int id)
        {
            var res = await _context.Uoms.FirstOrDefaultAsync(s => s.UomId == id);
            return res;
        }

        [HttpPut, Route("api/Uom/update")]
        public async Task<IActionResult> Update(Uom entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Uoms.FirstOrDefaultAsync(x => x.UomId == entity.UomId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
