using System.ComponentModel.DataAnnotations;

namespace Prog2WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [MaxLength(50)] 
        public required string Username { get; set; }
        
        [MaxLength(50)]
        public string? Password { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
