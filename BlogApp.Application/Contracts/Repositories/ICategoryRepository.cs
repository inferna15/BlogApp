using BlogApp.Domain.Entities;
using System.Linq.Expressions;

namespace BlogApp.Application.Contracts.Repositories
{
    public interface ICategoryRepository
    {
        Task<bool> CategoryExistsAsync(Expression<Func<Category, bool>> predicate);
        Task CreateAsync(Category category);
        Task SaveChangesAsync();
        Task<Category?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<Category>> GetAllAsync();
    }
}
