using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Posts
{
    public static class GetAllPosts
    {
        public record Query : IRequest<Result<List<Dto>>>;

        public record Dto(
            int Id,
            string Title,
            string Content,
            int UserId,
            string UserName,
            int CategoryId,
            string CategoryName
        );

        public class Handler(IPostRepository repository) : IRequestHandler<Query, Result<List<Dto>>>
        {
            public async Task<Result<List<Dto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var posts = await repository.GetAllWithDetailsAsync();

                var dtos = posts.Select(p => new Dto(
                    p.Id,
                    p.Title,
                    p.Content,
                    p.User.Id,
                    p.User.UserName,
                    p.Category.Id,
                    p.Category.Name
                )).ToList();

                return Result<List<Dto>>.Success(dtos);
            }
        }
    }
}
