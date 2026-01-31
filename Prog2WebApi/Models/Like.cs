namespace Prog2WebApi.Models
{
    public class Like
    {
        public int Id { get; set; }

        // divas FOREIGN KEY
        public int UserId { get; set; }
        public int PostId { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
