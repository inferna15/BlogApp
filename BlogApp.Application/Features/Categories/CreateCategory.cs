using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Categories
{
    public static class CreateCategory
    {
        public record Command(string Name) : IRequest<Result>;
        public class Handler(ICategoryRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var category = new Category(request.Name);
                await repository.CreateAsync(category);
                await repository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
