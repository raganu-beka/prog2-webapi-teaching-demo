using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prog2WebApi.Data;
using Prog2WebApi.Models;
using Prog2WebApi.Models.Requests;
using Prog2WebApi.Models.Responses;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Prog2WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPosts()
        {
            var allPosts = dbContext.Posts
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    LikeCount = p.Likes.Count,
                    Author = new UserResponse
                    {
                        Id = p.UserId,
                        Username = p.User!.Username
                    },
                    Comments = p.Comments.Select(c => new CommentResponse
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        Author = c.User != null ? new UserResponse
                        {
                            Id = c.UserId,
                            Username = c.User.Username
                        } : null
                    }).ToList()
                })
                .ToList();

            return Ok(allPosts);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPostById(int id)
        {
            var post = dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Id == id);
            
            if (post == null) return NotFound();

            return Ok(new PostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                LikeCount = post.LikeCount,
                Author = new UserResponse
                {
                    Id = post.UserId,
                    Username = post.User!.Username
                },
                Comments = post.Comments.Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Author = c.User != null ? new UserResponse
                    {
                        Id = c.UserId,
                        Username = c.User.Username
                    } : null
                }).ToList()
            });
        }

        // atribūts Authorize. lietotājam jābūt autorizētam
        // ja ir šis atribūts - no tokena var izgūt info par lietotāju
        [Authorize]
        [HttpPost]
        public IActionResult CreatePost(PostRequest request)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized();
            }
            
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();

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

            var existingLike = dbContext.Likes.FirstOrDefault(
                l => l.UserId == userId && l.PostId == postId);

            if (existingLike == null)
            {
                var like = new Like
                {
                    PostId = postId,
                    UserId = userId,
                };
                dbContext.Likes.Add(like);
                dbContext.SaveChanges();

                return Ok(new { msg = "Like added." });
            }

            dbContext.Likes.Remove(existingLike);
            dbContext.SaveChanges();

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

            dbContext.Comment.Add(comment);
            dbContext.SaveChanges();

            return Ok(new { id = comment.Id });
        }
    }
}
