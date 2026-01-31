using System.ComponentModel.DataAnnotations;

namespace Prog2WebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        
        [MaxLength(500)]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // divas FOREIGN KEY
        public int UserId { get; set; }
        public int PostId { get; set; }

        public User? User { get; set; }
        public Post? Post { get; set; }
    }
}
