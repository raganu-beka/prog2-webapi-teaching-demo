namespace Prog2WebApi.Models.Responses;

public class CommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserResponse? Author { get; set; }
}