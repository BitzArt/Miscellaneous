using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ProductsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _db.Products.ToListAsync();
            var result = products.Select(x => new ProductDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid productId)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product is null) return NotFound();
            var result = new ProductDisplayViewModel(product);
            return Ok(result);
        }
    }
}
