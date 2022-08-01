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

        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid blogId)
        {
            var blog = await _db.Blogs
                .Where(x => x.Id == blogId)
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

        [HttpPut("{blogId}/posts")]
        public async Task<IActionResult> ConfigureBlogPostsAsync([FromRoute] Guid blogId, [FromBody] ConfigureBlogPostsRequest request)
        {
            var blog = await _db.Blogs
                .Include(x => x.Posts)
                .Where(x => x.Id == blogId)
                .FirstOrDefaultAsync();
            if (blog is null) return NotFound();

            blog.Posts!.Clear();

            // Normally you should not do db calls in a loop.
            // This is for nuget package demonstration purposes only.
            foreach (var postId in request.PostIds)
            {
                var post = await _db.Posts.FindAsync(postId);
                if (post is null) return NotFound();
                blog.Posts.Add(post);
            }
            await _db.SaveChangesAsync();

            var result = new BlogDisplayViewModel(blog);
            return Ok(result);
        }
    }
}
