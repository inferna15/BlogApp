using BlogApp.Domain.Entities;
using System.Security.Claims;

namespace BlogApp.Application.Common
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user, string role);
    }
}
