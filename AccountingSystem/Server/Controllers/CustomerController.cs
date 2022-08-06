using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public CustomerController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Customer/getCustomers")]
        public async Task<List<Customer>> GetCustomersAsync()
        {
            var res = await _context.Customers.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Customer/create")]
        public async Task<bool> Create([FromBody] Customer entity)
        {
            await _context.Customers.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Customer/getdetails/{id}")]
        public async Task<Customer> GetDetails(int id)
        {
            var res = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerId == id);
            return res;
        }

        [HttpPut, Route("api/Customer/update")]
        public async Task<IActionResult> Update(Customer entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == entity.CustomerId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
