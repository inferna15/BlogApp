using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : IdentityDbContext<User, IdentityRole<int>, int>(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
