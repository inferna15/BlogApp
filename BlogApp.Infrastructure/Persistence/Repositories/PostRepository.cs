using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class PostRepository(AppDbContext context) : IPostRepository
    {
        public async Task CreateAsync(Post post)
        {
            await context.Posts.AddAsync(post);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await context.Posts.FindAsync(id);
            context.Posts.Remove(post!);
        }

        public async Task<List<Post>> GetAllWithDetailsAsync()
        {
            return await context.Posts
                .Include(x => x.User)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<List<Post>> GetByCategoryIdAsync(int categoryId)
        {
            return await context.Posts
                .Include(x => x.User)
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await context.Posts.FindAsync(id);
        }

        public async Task<Post?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Posts
                .Include(x => x.User)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Post>> GetByUserIdAsync(int userId)
        {
            return await context.Posts
                .Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> PostExistsAsync(Expression<Func<Post, bool>> predicate)
        {
            return await context.Posts.AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
