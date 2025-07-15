using Microsoft.AspNetCore.Identity;

namespace BlogsAPI.Models.DTOs
{
    public class BlogDto
    {
        public int blogId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string? User { get; set; }
     
        public ICollection<CommentDto> Comments { get; set; }
        public DateTime Created { get; set; }
    }
}