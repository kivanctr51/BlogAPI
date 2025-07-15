using System.Net;
using System.Net.Mail;
using BlogsAPI.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Roller;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
{
    // yayında bunları yapmayın lütfen :)
    opt.Password.RequiredLength = 1;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequiredUniqueChars = 0;
    opt.Password.RequireDigit = false;
    opt.Password.RequiredUniqueChars = 0;
    opt.SignIn.RequireConfirmedEmail = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
if (smtpSettings != null)
    builder.Services
        .AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
        .AddRazorRenderer()
        .AddSmtpSender(new SmtpClient(smtpSettings.Host, smtpSettings.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
        });

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy
            //.WithOrigins("http://localhost:5173")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            //.AllowCredentials()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseRouting();
app.UseHttpsRedirection();


app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.MapGroup("Auth").MapIdentityApi<IdentityUser>().WithTags("Auth");
app.Run();