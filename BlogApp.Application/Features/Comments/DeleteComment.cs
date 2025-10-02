using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Comments
{
    public static class DeleteComment
    {
        public record Command(int Id, int UserId) : IRequest<Result>;
        public class Handler(ICommentRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));

                await repository.DeleteAsync(request.Id);
                return Result.Success();
            }
        }
    }
}
