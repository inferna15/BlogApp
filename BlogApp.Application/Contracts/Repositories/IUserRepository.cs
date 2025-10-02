using BlogApp.Domain.Entities;
using System.Linq.Expressions;

namespace BlogApp.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(Expression<Func<User, bool>> predicate);
    }
}
