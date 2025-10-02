using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Comments
{
    public static class UpdateComment
    {
        public record Command(int Id, int UserId, string Content) : IRequest<Result>;
        public class Handler(ICommentRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var comment = await repository.GetByIdAsync(request.Id);

                comment.Update(request.Content);
                await repository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
