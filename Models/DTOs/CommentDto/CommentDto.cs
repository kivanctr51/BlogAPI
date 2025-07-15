namespace BlogsAPI.Models.DTOs;

public class CommentDto
{
    public int CommentId { get; set; }
    
    public string Content { get; set; }    
    public DateTime created_at { get; set; }
    public string? UserName { get; set; }
    
}