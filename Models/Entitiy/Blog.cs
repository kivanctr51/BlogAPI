using Microsoft.AspNetCore.Identity;

namespace BlogsAPI.Models;

public class Blog
{
    public int blogId;
    public int Id { get; set; }
    public IdentityUser User { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public DateTime created { get; set; } = DateTime.Now;
    

    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}