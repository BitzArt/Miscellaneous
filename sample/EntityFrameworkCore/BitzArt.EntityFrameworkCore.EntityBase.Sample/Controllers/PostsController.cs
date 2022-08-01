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
            var posts = await _db.Posts.ToListAsync();
            var result = posts.Select(x => new PostDisplayViewModel(x));
            return Ok(result);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid postId)
        {
            var post = await _db.Posts.FindAsync(postId);
            if (post is null) return NotFound();
            var result = new PostDisplayViewModel(post);
            return Ok(result);
        }
    }
}
