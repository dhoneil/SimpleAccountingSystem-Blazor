using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class BranchController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public BranchController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Branch/getBranchs")]
        public async Task<List<Branch>> GetBranchsAsync()
        {
            var res = await _context.Branches.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Branch/create")]
        public async Task<bool> Create([FromBody] Branch entity)
        {
            await _context.Branches.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Branch/getdetails/{id}")]
        public async Task<Branch> GetDetails(int id)
        {
            var res = await _context.Branches.FirstOrDefaultAsync(s => s.BranchId == id);
            return res;
        }

        [HttpPut, Route("api/Branch/update")]
        public async Task<IActionResult> Update(Branch entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Branches.FirstOrDefaultAsync(x => x.BranchId == entity.BranchId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
