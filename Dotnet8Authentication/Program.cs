using Dotnet8Authentication.Database;
using Dotnet8Authentication.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigration();
}

app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext ctx) =>
{
    string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    return await ctx.Users.FindAsync(userId);
})
   .RequireAuthorization();

//app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
//{
//    string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

//    return await context.Users.FindAsync(userId);
//})
//.RequireAuthorization();


app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.Run();

