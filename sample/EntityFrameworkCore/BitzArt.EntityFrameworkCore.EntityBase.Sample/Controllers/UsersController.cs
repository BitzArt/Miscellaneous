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

        [HttpPost("{userId}/posts")]
        public async Task<IActionResult> AddPostAsync([FromRoute] Guid userId, [FromBody] AddPostRequest request)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var post = request.ToPost(user);
            _db.Add(post);
            await _db.SaveChangesAsync();

            var result = new PostDisplayViewModel(post);
            return Ok(result);
        }

        [HttpPatch("{userId}/posts/{postId}")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] Guid userId, [FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return NotFound();

            var post = await _db.Posts.FindAsync(postId);
            if (post is null) return NotFound();

            post = request.Apply(post, user);
            await _db.SaveChangesAsync();

            var result = new PostDisplayViewModel(post);
            return Ok(result);
        }
    }
}
