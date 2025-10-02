using BlogApp.Application.Contracts.Repositories;
using BlogApp.Application.Features.Categories;
using FluentValidation;

namespace BlogApp.Application.Validators.Categories
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategory.Command>
    {
        public DeleteCategoryValidator(ICategoryRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category ID.")
                .MustAsync(async (id, cancellationToken) =>
                    await repository.CategoryExistsAsync(c => c.Id == id))
                .WithMessage("Category not found.");
        }
    }
}
