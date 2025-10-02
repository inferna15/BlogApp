using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class CommentRepository(AppDbContext context) : ICommentRepository
    {
        public async Task<bool> CommentExistsAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await context.Comments.AnyAsync(predicate);
        }

        public async Task CreateAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            context.Comments.Remove(comment!);
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await context.Comments.FindAsync(id);
        }

        public Task<List<Comment>> GetByPostIdAsync(int postId)
        {
            return context.Comments
                .Include(x => x.User)
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
