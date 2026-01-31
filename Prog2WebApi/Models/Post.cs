using Prog2WebApi.Models.Requests;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prog2WebApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign key - atsaucas uz citu tabulu (Users)
        public int UserId { get; set; }

        // šis ļaus izgūt User objektu no Post
        public User User { get; set; }
        // šis ļaus izgūt sarakstu ar Likes no Post
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // lauks tiek aprēķināts automātiski, netiek glabāts datubāzē.
        [NotMapped]
        public int LikeCount => Likes?.Count ?? 0;

        public static Post From(PostRequest dto, int userId)
        {
            return new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }
    }
}
