using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<bool> UserExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users.AnyAsync(predicate);
        }
    }
}
