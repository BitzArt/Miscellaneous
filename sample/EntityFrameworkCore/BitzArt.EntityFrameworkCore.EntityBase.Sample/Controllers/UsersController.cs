using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _db;

        public UsersController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _db.Users.ToListAsync();
            var result = users.Select(x => new UserDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var result = new UserDisplayViewModel(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddUserRequest request)
        {
            var user = request.ToUser();
            _db.Add(user);
            await _db.SaveChangesAsync();
            var result = new UserDisplayViewModel(user);
            return Ok(result);
        }

        [HttpPost("{userId}/products")]
        public async Task<IActionResult> AddProductAsync([FromRoute] Guid userId, [FromBody] AddProductRequest request)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var product = request.ToProduct(user);
            _db.Add(product);
            await _db.SaveChangesAsync();

            var result = new ProductDisplayViewModel(product);
            return Ok(result);
        }

        [HttpPatch("{userId}/products/{productId}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid userId, [FromRoute] Guid productId, [FromBody] UpdateProductRequest request)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var product = await _db.Products.FindAsync(productId);
            if (product is null) return NotFound();

            product = request.Apply(product, user);
            await _db.SaveChangesAsync();

            var result = new ProductDisplayViewModel(product);
            return Ok(result);
        }
    }
}
