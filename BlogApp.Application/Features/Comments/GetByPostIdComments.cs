using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Comments
{
    public static class GetByPostIdComments
    {
        public record Query(int PostId) : IRequest<Result<List<Dto>>>;
        public record Dto(int Id, string Content, int UserId, string UserName);
        public class Handler(ICommentRepository repository) : IRequestHandler<Query, Result<List<Dto>>>
        {
            public async Task<Result<List<Dto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await repository.GetByPostIdAsync(request.PostId);
                var dtos = comments.Select(c => new Dto(
                    c.Id,
                    c.Content,
                    c.UserId,
                    c.User.UserName
                )).ToList();
                return Result<List<Dto>>.Success(dtos);
            }
        }
    }
}
