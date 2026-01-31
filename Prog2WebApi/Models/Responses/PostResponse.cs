namespace Prog2WebApi.Models.Responses;

public class PostResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public UserResponse? Author { get; set; }
    public int LikeCount { get; set; }
    public List<CommentResponse> Comments { get; set; } = [];
}