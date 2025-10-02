using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Categories
{
    public static class GetAllCategories
    {
        public record Query : IRequest<Result<List<Dto>>>;
        public record Dto(int Id, string Name);

        public class Handler(ICategoryRepository repository) : IRequestHandler<Query, Result<List<Dto>>>
        {
            public async Task<Result<List<Dto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await repository.GetAllAsync();
                var dtos = categories.Select(c => new Dto(c.Id, c.Name)).ToList();
                return Result<List<Dto>>.Success(dtos);
            }
        }
    }
}
