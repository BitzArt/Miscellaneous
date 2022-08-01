using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using BitzArt.EntityFrameworkCore.EntityBase.Sample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public BlogsController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var blogs = await _db.Blogs.ToListAsync();
            var result = blogs.Select(x => new BlogDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid categoryId)
        {
            var blog = await _db.Blogs
                .Where(x => x.Id == categoryId)
                .Include(x => x.Posts)
                .FirstOrDefaultAsync();
            if (blog is null) return NotFound();

            var result = new BlogDisplayViewModel(blog);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddBlogRequest request)
        {
            var blog = request.ToBlog();
            _db.Add(blog);
            await _db.SaveChangesAsync();
            var result = new BlogDisplayViewModel(blog);
            return Ok(result);
        }

        [HttpPut("{categoryId}/products")]
        public async Task<IActionResult> ConfigureCategoryProductsAsync([FromRoute] Guid categoryId, [FromBody] ConfigureBlogPostsRequest request)
        {
            var blog = await _db
                .Blogs
                .Include(x => x.Posts)
                .Where(x => x.Id == categoryId)
                .FirstOrDefaultAsync();
            if (blog is null) return NotFound();

            blog.Posts!.Clear();

            foreach(var productId in request.PostIds)
            {
                var post = await _db.Posts.FindAsync(productId);
                if (post is null) return NotFound();
                blog.Posts.Add(post);
            }
            await _db.SaveChangesAsync();

            var result = new BlogDisplayViewModel(blog);
            return Ok(result);
        }
    }
}
