using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<bool> CategoryExistsAsync(Expression<Func<Category, bool>> predicate)
        {
            return await context.Categories.AnyAsync(predicate);
        }

        public async Task CreateAsync(Category category)
        {
            await context.Categories.AddAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);
            context.Categories.Remove(category!);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories.FindAsync(id);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
