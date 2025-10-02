using BlogApp.Application.Features.Categories;
using BlogApp.Application.Features.Posts;
using BlogApp.Presentation.Dtos.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Presentation.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDto dto)
        {
            var command = new CreateCategory.Command(dto.Name);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Created();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryDto dto)
        {
            var command = new UpdateCategory.Command(id, dto.Name);
            var result = await sender.Send(command);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var command = new DeleteCategory.Command(id);
            var result = await sender.Send(command);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetAllCategories.Query();
            var result = await sender.Send(query);
            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var query = new GetByIdCategory.Query(id);
            var result = await sender.Send(query);
            if (result is not null)
                return Ok(result.Data);
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("{category_id}/posts")]
        public async Task<IActionResult> GetByCategoryIdAsync(int category_id)
        {
            var query = new GetByCategoryIdPosts.Query(category_id);
            var result = await sender.Send(query);

            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return NotFound(result.ErrorMessage);
        }
    }
}
