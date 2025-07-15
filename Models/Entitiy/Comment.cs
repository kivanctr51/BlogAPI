using System.ComponentModel.DataAnnotations;
using BlogsAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BlogsAPI.Models;

public class Comment
{
    public int Id { get; set; }

    public string Content { get; set; }
    public DateTime created_at { get; set; }
    

    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    
    public Blog Blog { get; set; }
    public int blogId { get; set; }
}