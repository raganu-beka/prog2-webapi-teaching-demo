using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prog2WebApi.Data;
using Prog2WebApi.Models;
using Prog2WebApi.Models.Requests;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Prog2WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _db;

        // šeit notiek AppDbContext injekcija
        public PostsController(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            var allPosts = _db.Posts
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    p.CreatedAt,
                    p.UserId,
                    LikeCount = p.Likes.Count
                })
                .ToList();
            return Ok(allPosts);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPostById(int id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // atribūts authorize. lietotājam jābūt autorizētam
        // ja ir šis atribūts - no tokena var izgūt info par lietotāju
        [Authorize]
        [HttpPost]
        public IActionResult CreatePost(PostRequest request)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            var post = Post.From(request, userId);
            _db.Posts.Add(post);
            _db.SaveChanges();

            return Ok(new { id = post.Id });
        }

        [Authorize]
        [HttpPost("{postId:int}/like")]
        public IActionResult Like(int postId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            var existingLike = _db.Likes.FirstOrDefault(
                l => l.UserId == userId && l.PostId == postId);

            if (existingLike == null)
            {
                var like = new Like
                {
                    PostId = postId,
                    UserId = userId,
                };
                _db.Likes.Add(like);
                _db.SaveChanges();

                return Ok(new { msg = "Like added." });
            }

            _db.Likes.Remove(existingLike);
            _db.SaveChanges();

            return Ok(new { msg = "Like removed." });
        }

        [Authorize]
        [HttpPost("{postId:int}/comment")]
        public IActionResult AddComment(int postId, [FromBody] CommentRequest request)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }

            var comment = new Comment
            {
                UserId = userId,
                PostId = postId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _db.Comment.Add(comment);
            _db.SaveChanges();

            return Ok(new { id = comment.Id });
        }
    }
}
