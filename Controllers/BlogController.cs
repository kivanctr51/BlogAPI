using BlogsAPI.Data;
using BlogsAPI.Models;
using BlogsAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlogsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BlogController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<int>(StatusCodes.Status400BadRequest)]
    [HttpPost("")]
   [Authorize]
    public IActionResult BlogAdd([FromBody] BlogCreateDto blogCreateDto)
    {
        var AddUser = _userManager.GetUserId(User);

        var blogs = new Blog
        {
            UserId = AddUser,
            Title = blogCreateDto.Title,
            Summary = blogCreateDto.Summary,
            Content = blogCreateDto.Content,
            created = DateTime.Now
        };

        _context.Add(blogs);
        _context.SaveChanges();
        return Ok(blogs);

    }

    [HttpGet("")]
    public async Task<IActionResult> BlogGet()
    {
        var blogs = await _context.blogs
            .Include(b => b.User) 
            .Include(b => b.Comments)
            .ThenInclude(c => c.User)
            .ToListAsync();
    
        var blogDtos = blogs.Select(b => new BlogDto
        {
            blogId = b.Id,
            Title = b.Title,
            Summary = b.Summary,
            Content = b.Content,
            Created = b.created,
            User = b.User.UserName,
            Comments = b.Comments.Select(c => new CommentDto
            {
                UserName = c.User.UserName,
                CommentId = c.Id,
                Content = c.Content,
                created_at = c.created_at
               
            }).ToList()
        }).ToList();
        return Ok(blogDtos);
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Blogdelete(int id)
    {
        var DeleteBlog = _context.blogs.Find(id);
        if (DeleteBlog != null)
        {
            _context.blogs.Remove(DeleteBlog);
            _context.SaveChanges();
            return Ok("blog silme Başarılı");
        }
            
        return Ok();
    }

   
}