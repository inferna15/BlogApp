using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Posts
{
    public static class GetByCategoryIdPosts
    {
        public record Query(int CategoryId) : IRequest<Result<List<Dto>>>;
        public record Dto(
            int Id,
            string Title,
            string Content,
            int UserId,
            string UserName
        );
        public class Handler(IPostRepository repository) : IRequestHandler<Query, Result<List<Dto>>>
        {
            public async Task<Result<List<Dto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var posts = await repository.GetByCategoryIdAsync(request.CategoryId);
                var dtos = posts.Select(p => new Dto(
                    p.Id,
                    p.Title,
                    p.Content,
                    p.User.Id,
                    p.User.UserName
                )).ToList();
                return Result<List<Dto>>.Success(dtos);
            }
        }
    }
}
