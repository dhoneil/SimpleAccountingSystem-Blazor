using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class ContractController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public ContractController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/contract/getallcontracts")]
        public async Task<List<Contract>> GetContractsAsync()
        {
            var res = await _context.Contracts.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/contract/createcontract")]
        public async Task<IActionResult> CreateContract([FromBody] Contract entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            await _context.AddAsync(entity);
            var res = await _context.SaveChangesAsync();

            if (res != 1)
                return BadRequest("Failed during inserting to database");

            return Ok(res);
        }

        [HttpPut, Route("api/contract/updatecontract")]
        public async Task<IActionResult> UpdateContract(Contract entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }

        [HttpGet, Route("api/contract/getdetails/{id}")]
        public async Task<Contract> GetDetails(int id)
        {
            var res = await _context.Contracts.FirstOrDefaultAsync(s => s.Id == id);
            return res;
        }
    }
}
