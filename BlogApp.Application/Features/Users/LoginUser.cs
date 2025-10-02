using BlogApp.Application.Common;
using BlogApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogApp.Application.Features.Users
{
    public static class LoginUser
    {
        public record Command(string Email, string Password) : IRequest<Result<string>>;

        public class Handler(UserManager<User> userManager, ITokenGenerator tokenGenerator) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                
                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Result<string>.Failure("Invalid email or password.");
                }

                var token = tokenGenerator.GenerateToken(user, (await userManager.GetRolesAsync(user)).First());

                return Result<string>.Success(token);
            }
        }
    }
}
