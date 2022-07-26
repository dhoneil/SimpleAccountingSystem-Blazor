using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class SupplierController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public SupplierController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Supplier/getSuppliers")]
        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            var res = await _context.Suppliers.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Supplier/create")]
        public async Task<bool> Create([FromBody] Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Supplier/getdetails/{id}")]
        public async Task<Supplier> GetDetails(int id)
        {
            var res = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);
            return res;
        }

        [HttpPut, Route("api/Supplier/update")]
        public async Task<IActionResult> Update(Supplier entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == entity.SupplierId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
