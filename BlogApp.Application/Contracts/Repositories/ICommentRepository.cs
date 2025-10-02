using BlogApp.Domain.Entities;
using System.Linq.Expressions;

namespace BlogApp.Application.Contracts.Repositories
{
    public interface ICommentRepository
    {
        Task<bool> CommentExistsAsync(Expression<Func<Comment, bool>> predicate);
        Task CreateAsync(Comment comment);
        Task SaveChangesAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<Comment>> GetByPostIdAsync(int postId);
    }
}
