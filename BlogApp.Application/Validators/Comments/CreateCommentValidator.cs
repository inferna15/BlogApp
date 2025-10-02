using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Comments;
using FluentValidation;

namespace BlogApp.Application.Validators.Comments
{
    public class CreateCommentValidator : AbstractValidator<CreateComment.Command>
    {
        public CreateCommentValidator(IUserRepository userRepository, IPostRepository postRepository)
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(1000)
                .WithMessage("Content must not exceed 1000 characters.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be a positive integer.")
                .MustAsync(async (userId, cancellation) => 
                    await userRepository.UserExistsAsync(x => x.Id == userId))
                .WithMessage("User does not exist.");

            RuleFor(x => x.PostId)
                .GreaterThan(0)
                .WithMessage("PostId must be a positive integer.")
                .MustAsync(async (postId, cancellation) => 
                    await postRepository.PostExistsAsync(x => x.Id == postId))
                .WithMessage("Post does not exist.");
        }
    }
}
