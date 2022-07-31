using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _db;

        public CategoriesController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            var result = categories.Select(x => new CategoryDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid categoryId)
        {
            var category = await _db.Categories
                .Where(x => x.Id == categoryId)
                .Include(x => x.Products)
                .FirstOrDefaultAsync();
            if (category is null) return NotFound();

            var result = new CategoryDisplayViewModel(category);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddCategoryRequest request)
        {
            var category = request.ToCategory();
            _db.Add(category);
            await _db.SaveChangesAsync();
            var result = new CategoryDisplayViewModel(category);
            return Ok(result);
        }

        [HttpPut("{categoryId}/products")]
        public async Task<IActionResult> ConfigureCategoryProductsAsync([FromRoute] Guid categoryId, [FromBody] ConfigureCategoryProductsRequest request)
        {
            var category = await _db
                .Categories
                .Include(x => x.Products)
                .Where(x => x.Id == categoryId)
                .FirstOrDefaultAsync();
            if (category is null) return NotFound();

            category.Products!.Clear();

            foreach(var productId in request.ProductIds)
            {
                var product = await _db.Products.FindAsync(productId);
                if (product is null) return NotFound();
                category.Products.Add(product);
            }
            await _db.SaveChangesAsync();

            var result = new CategoryDisplayViewModel(category);
            return Ok(result);
        }
    }
}
