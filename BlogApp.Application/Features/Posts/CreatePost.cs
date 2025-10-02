using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using BlogApp.Domain.Entities;
using FluentValidation;
using MediatR;

namespace BlogApp.Application.Features.Posts
{
    public static class CreatePost
    {
        public record Command(string Title, string Content, int UserId, int CategoryId) : IRequest<Result>;

        public class Handler(IPostRepository postRepository, IValidator<Command> validator) 
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);

                if (!result.IsValid)
                    return Result.Failure(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));

                var post = new Post(request.Title, request.Content, request.UserId, request.CategoryId);

                await postRepository.CreateAsync(post);
                await postRepository.SaveChangesAsync();
                return Result.Success();
            }
        }
    }
}
