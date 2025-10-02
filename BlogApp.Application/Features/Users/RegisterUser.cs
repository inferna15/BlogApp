using BlogApp.Application.Common;
using BlogApp.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Application.Features.Users
{
    public static class RegisterUser
    {
        public record Command(string Username, string Email, string Password) : IRequest<Result>;

        public class Handler(UserManager<User> userManager, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var user = new User
                {
                    Email = request.Email,
                    UserName = request.Username
                };

                var userResult = await userManager.CreateAsync(user, request.Password);

                if (!userResult.Succeeded)
                    return Result.Failure(string.Join("; ", userResult.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(user, "User");

                return Result.Success();
            }
        }
    }
}
