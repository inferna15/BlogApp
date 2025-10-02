using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Comments
{
    public static class CreateComment
    {
        public record Command(int UserId, int PostId, string Content) : IRequest<Result>;
        public class Handler(ICommentRepository repository, IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var comment = new Comment(request.Content, request.UserId, request.PostId);
                await repository.CreateAsync(comment);
                await repository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
