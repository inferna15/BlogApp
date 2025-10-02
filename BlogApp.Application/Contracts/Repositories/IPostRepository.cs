using BlogApp.Domain.Entities;
using System.Linq.Expressions;

namespace BlogApp.Application.Contracts.Repositories
{
    public interface IPostRepository
    {
        Task<bool> PostExistsAsync(Expression<Func<Post, bool>> predicate);
        Task CreateAsync(Post post);
        Task SaveChangesAsync();
        Task<Post?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<Post?> GetByIdWithDetailsAsync(int id);
        Task<List<Post>> GetAllWithDetailsAsync();
        Task<List<Post>> GetByUserIdAsync(int userId);
        Task<List<Post>> GetByCategoryIdAsync(int categoryId);
    }
}
