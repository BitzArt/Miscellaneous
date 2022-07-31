using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ConfigController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("recreate")]
        public async Task<IActionResult> RecreateDbAsync()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            return Ok();
        }
    }
}
