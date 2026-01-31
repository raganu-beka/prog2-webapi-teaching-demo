using Prog2WebApi.Models.Requests;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prog2WebApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        [MaxLength(200)]    
        public required string Title { get; set; }
        
        [MaxLength(5000)]    
        public required string Content { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Foreign key - atsaucas uz citu tabulu (Users)
        public int UserId { get; set; }

        // šis ļaus izgūt User objektu no Post
        public User? User { get; set; }
        
        // šis ļaus izgūt sarakstu ar Likes no Post
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // lauks tiek aprēķināts automātiski, netiek glabāts datubāzē.
        [NotMapped]
        public int LikeCount => Likes?.Count ?? 0;
    }
}
