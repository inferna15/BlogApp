using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Categories
{
    public static class DeleteCategory
    {
        public record Command(int Id) : IRequest<Result>;
        public class Handler(ICategoryRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                await repository.DeleteAsync(request.Id);
                await repository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
