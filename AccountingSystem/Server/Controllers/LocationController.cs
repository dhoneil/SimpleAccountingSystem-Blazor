using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Server.DataAccess;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace AccountingSystem.Server.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {
        public readonly accounting_systemContext _context;
        public LocationController(accounting_systemContext context)
        {
            _context = context;
        }

        [HttpGet, Route("api/location/getlocations")]
        public async Task<List<Location>> GetLocationsAsync()
        {
            var res = await _context.Locations.ToListAsync();
            return res;
        }

        [HttpPost, Route("api/location/create")]
        public async Task<bool> Create([FromBody] Location entity)
        {
            await _context.Locations.AddAsync(entity);
            var res = await _context.SaveChangesAsync();
            return res == 0 ? false : true;
        }

        [HttpGet, Route("api/location/getdetails/{id}")]
        public async Task<Location> GetDetails(int id)
        {
            var res = await _context.Locations.FirstOrDefaultAsync(s => s.LocationId == id);
            return res;
        }

        [HttpPut, Route("api/location/update")]
        public async Task<IActionResult> Update(Location entity)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _context.Locations.FirstOrDefaultAsync(x => x.LocationId == entity.LocationId);
            _context.Entry(result).CurrentValues.SetValues(entity);
            var affectedRows = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!affectedRows)
                return BadRequest("Failed during inserting to database");

            return Ok();
        }
    }
}
