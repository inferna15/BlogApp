using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Categories;
using FluentValidation;

namespace BlogApp.Application.Validators.Categories
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory.Command>
    {
        public UpdateCategoryValidator(ICategoryRepository repository) 
        { 
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category ID.")
                .MustAsync(async (id, cancellationToken) => 
                    await repository.CategoryExistsAsync(c => c.Id == id))
                .WithMessage("Category not found.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.")
                .MustAsync(async (command, name, cancellationToken) => 
                    !await repository.CategoryExistsAsync(c => c.Name == name && c.Id != command.Id))
                .WithMessage("A category with the same name already exists.");
        }
    }
}
