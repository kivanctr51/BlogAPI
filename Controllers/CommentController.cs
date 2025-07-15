using System.Security.Claims;
using BlogsAPI.Data;

using BlogsAPI.Models;
using BlogsAPI.Models.DTOs;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Roller.Models.Emails;

namespace BlogsAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase 
{
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;
    
    
    public CommentController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
     
    }
    
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> CommentAdd( CommentCreateDto CommentCreateDto,IFluentEmail fluentEmail)
    {
        var AddUser = _userManager.GetUserId(User);
        

        var comment = new Comment
        {
            blogId = CommentCreateDto.BlogId,
            Content = CommentCreateDto.Content,
            created_at = DateTime.Now,
            UserId = AddUser,
        };
        _context.Add(comment);
        _context.SaveChanges();
        
        var userEmail = _userManager.GetUserName(User);

        
        var model = new WelcomeEmail
        {
            Name = "Bloguna Yorum geldi!!!",
            Message = "Hemen Sayfana Gelip yorumu Okuyabilirsin"
        };
        var templatePath = Path.Combine(AppContext.BaseDirectory, "Emails", "Welcome.cshtml");
        var email = fluentEmail
            .To(userEmail)
            .Subject("Blog Yazınıza yorum geldi!!!")
            .UsingTemplateFromFile(templatePath, model)
            .Send();
        return Ok("yorumunuz Başarıyla eklendi");
    }
    
    [HttpDelete("{id}")]
        [Authorize]
        public IActionResult commentdelete(int id)
        {
            var deleteComment = _context.comments.Find(id);
            if (deleteComment != null)
            {
                _context.comments.Remove(deleteComment);
                _context.SaveChanges();
                return Ok("yorum silme Başarılı");
            }
            
            return Ok();
        }

}