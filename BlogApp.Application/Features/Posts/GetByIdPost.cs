using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Posts
{
    public static class GetByIdPost
    {
        public record Query(int Id) : IRequest<Result<Dto>>;
        public record Dto(
            int Id,
            string Title,
            string Content,
            int UserId,
            string UserName,
            int CategoryId,
            string CategoryName
        );

        public class Handler(IPostRepository repository) : IRequestHandler<Query, Result<Dto>>
        {
            public async Task<Result<Dto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var post = await repository.GetByIdWithDetailsAsync(request.Id);

                if (post == null)
                {
                    return Result<Dto>.Failure("Post not found");
                }

                var dto = new Dto(
                    post.Id,
                    post.Title,
                    post.Content,
                    post.User.Id,
                    post.User.UserName,
                    post.Category.Id,
                    post.Category.Name
                );

                return Result<Dto>.Success(dto);
            }
        }
    }
}
