using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class BrandController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public BrandController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Brand/getBrands")]
        public async Task<List<Brand>> GetBrandsAsync()
        {
            var res = await _context.Brands.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Brand/create")]
        public async Task<bool> Create([FromBody] Brand entity)
        {
            await _context.Brands.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Brand/getdetails/{id}")]
        public async Task<Brand> GetDetails(int id)
        {
            var res = await _context.Brands.FirstOrDefaultAsync(s => s.BrandId == id);
            return res;
        }

        [HttpPut, Route("api/Brand/update")]
        public async Task<IActionResult> Update(Brand entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == entity.BrandId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
