using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Categories;
using FluentValidation;

namespace BlogApp.Application.Validators.Categories
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategory.Command>
    {
        public CreateCategoryValidator(ICategoryRepository repository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.")
                .MustAsync(async (name, cancellation) => 
                    !await repository.CategoryExistsAsync(c => c.Name == name))
                .WithMessage("Category name must be unique.");
        }
    }
}
