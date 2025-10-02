using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Comments;
using FluentValidation;

namespace BlogApp.Application.Validators.Comments
{
    public class UpdateCommentValidator :AbstractValidator<UpdateComment.Command>
    {
        public UpdateCommentValidator(ICommentRepository repository, ICurrentUserService service)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Comment ID must be greater than zero.")
                .MustAsync(async (id, cancellationToken) => 
                    await repository.CommentExistsAsync(c => c.Id == id))
                .WithMessage("Comment with the given ID does not exist.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MustAsync(async (command, userId, cancellationToken) =>
                {
                    if (service.IsInRole("Admin"))
                        return true;
                    return await repository.CommentExistsAsync(p => p.Id == command.Id && p.UserId == userId);
                })
                .WithMessage("You are not authorized to update this post.");


            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content must not be empty.")
                .MaximumLength(1000)
                .WithMessage("Content must not exceed 1000 characters.");
        }
    }
}
