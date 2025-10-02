using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Categories
{
    public static class UpdateCategory
    {
        public record Command(int Id, string Name) : IRequest<Result>;

        public class Handler(ICategoryRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var category = await repository.GetByIdAsync(request.Id);

                category.Update(request.Name);
                await repository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
