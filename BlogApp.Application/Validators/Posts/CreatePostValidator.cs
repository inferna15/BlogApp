using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Posts;
using FluentValidation;

namespace BlogApp.Application.Validators.Posts
{
    public class CreatePostValidator : AbstractValidator<CreatePost.Command>
    {
        public CreatePostValidator(IPostRepository postRepository, 
                                   IUserRepository userRepository, 
                                   ICategoryRepository categoryRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.")
                .MustAsync(async (title, cancellation) => 
                    !await postRepository.PostExistsAsync(p => p.Title == title))
                .WithMessage("A post with the same title already exists.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(5000)
                .WithMessage("Content must not exceed 5000 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MustAsync(async (userId, cancellation) => 
                    await userRepository.UserExistsAsync(u => u.Id == userId))
                .WithMessage("The specified user does not exist.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required.")
                .MustAsync(async (categoryId, cancellation) => 
                    await categoryRepository.CategoryExistsAsync(c => c.Id == categoryId))
                .WithMessage("The specified category does not exist.");
        }
    }
}
