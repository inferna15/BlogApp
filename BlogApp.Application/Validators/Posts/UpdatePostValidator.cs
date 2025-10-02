using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Posts;
using FluentValidation;

namespace BlogApp.Application.Validators.Posts
{
    public class UpdatePostValidator : AbstractValidator<UpdatePost.Command>
    {
        public UpdatePostValidator(IPostRepository postRepository, 
                                   ICategoryRepository categoryRepository,
                                   ICurrentUserService service)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.")
                .MustAsync(async (id, cancellationToken) =>
                    await postRepository.PostExistsAsync(p => p.Id == id))
                .WithMessage("The specified post does not exist.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.")
                .MustAsync(async (command, title, cancellationToken) =>
                    !await postRepository.PostExistsAsync(p => p.Title == title && p.Id != command.Id))
                .WithMessage("A post with the same title already exists.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(5000)
                .WithMessage("Content must not exceed 5000 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .MustAsync(async (command, userId, cancellationToken) =>
                {
                    if (service.IsInRole("Admin"))
                        return true;
                    return await postRepository.PostExistsAsync(p => p.Id == command.Id && p.UserId == userId);
                })    
                .WithMessage("You are not authorized to update this post.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required.")
                .MustAsync(async (categoryId, cancellationToken) =>
                    await categoryRepository.CategoryExistsAsync(c => c.Id == categoryId))
                .WithMessage("The specified category does not exist.");
        }
    }
}
