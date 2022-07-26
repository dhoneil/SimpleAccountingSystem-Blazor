using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public CategoryController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/Category/getCategories")]
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var res = await _context.Categories.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/Category/create")]
        public async Task<bool> Create([FromBody] Category entity)
        {
            await _context.Categories.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/Category/getdetails/{id}")]
        public async Task<Category> GetDetails(int id)
        {
            var res = await _context.Categories.FirstOrDefaultAsync(s => s.CategoryId == id);
            return res;
        }

        [HttpPut, Route("api/Category/update")]
        public async Task<IActionResult> Update(Category entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == entity.CategoryId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
