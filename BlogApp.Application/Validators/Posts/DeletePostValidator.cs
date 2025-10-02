using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Posts;
using FluentValidation;

namespace BlogApp.Application.Validators.Posts
{
    public class DeletePostValidator : AbstractValidator<DeletePost.Command>
    {
        public DeletePostValidator(IPostRepository repository, ICurrentUserService service)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.")
                .MustAsync(async (id, cancellationToken) =>
                    await repository.PostExistsAsync(p => p.Id == id))
                .WithMessage("The specified post does not exist.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MustAsync(async (command, userId, cancellationToken) =>
                {
                    if (service.IsInRole("Admin"))
                        return true;
                    return await repository.PostExistsAsync(p => p.Id == command.Id && p.UserId == userId);
                })
                .WithMessage("You are not authorized to delete this post.");
        }
    }
}
