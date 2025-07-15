namespace BlogsAPI.Models.DTOs;

public class CommentCreateDto
{
    public int BlogId { get; set; }
 
    public string Content { get; set; }    
   
}