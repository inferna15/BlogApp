using BlogApp.Application.Common;
using BlogApp.Application.Contracts.Repositories;
using MediatR;

namespace BlogApp.Application.Features.Categories
{
    public static class GetByIdCategory
    {
        public record Query(int Id) : IRequest<Result<Dto>>;
        public record Dto(int Id, string Name);

        public class Handler(ICategoryRepository repository) : IRequestHandler<Query, Result<Dto>>
        {
            public async Task<Result<Dto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await repository.GetByIdAsync(request.Id);
                if (category is null)
                {
                    return Result<Dto>.Failure("Category not found.");
                }
                var dto = new Dto(category.Id, category.Name);
                return Result<Dto>.Success(dto);
            }
        }
    }
}
