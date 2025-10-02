using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Posts
{
    public static class UpdatePost
    {
        public record Command(int Id, string Title, string Content, int UserId, int CategoryId) : IRequest<Result>;

        public class Handler(IPostRepository postRepository, IValidator<Command> validator)
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var post = await postRepository.GetByIdAsync(request.Id);

                post.Update(request.Title, request.Content, request.CategoryId);

                await postRepository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
