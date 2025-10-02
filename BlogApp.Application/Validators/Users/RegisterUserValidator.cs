using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Users;
using FluentValidation;

namespace BlogApp.Application.Validators.Users
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser.Command>
    {
        public RegisterUserValidator(IUserRepository repository)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid email is required.")
                .MustAsync(async (email, cancellation) => 
                    !await repository.UserExistsAsync(u => u.Email == email))
                .WithMessage("Email is already in use.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(20)
                .WithMessage("Username must not exceed 20 characters.")
                .MustAsync(async (username, cancellation) => 
                    !await repository.UserExistsAsync(u => u.UserName == username))
                .WithMessage("Username is already in use.");
        }
    }
}
