using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PostsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public PostsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _db.Posts.ToListAsync();
            var result = products.Select(x => new PostDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid productId)
        {
            var product = await _db.Posts.FindAsync(productId);
            if (product is null) return NotFound();
            var result = new PostDisplayViewModel(product);
            return Ok(result);
        }
    }
}
