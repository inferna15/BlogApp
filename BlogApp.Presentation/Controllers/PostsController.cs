using BlogApp.Application.Common;
using BlogApp.Application.Features.Comments;
using BlogApp.Application.Features.Posts;
using BlogApp.Presentation.Dtos.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Presentation.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController(ISender sender, ICurrentUserService service) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePostDto dto)
        {
            var command = new CreatePost.Command(dto.Title, dto.Title, service.UserId, dto.CategoryId);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Created();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody]     UpdatePostDto dto, int id)
        {
            var command = new UpdatePost.Command(id, dto.Title, dto.Content, service.UserId, dto.CategoryId);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var command = new DeletePost.Command(id, service.UserId);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var query = new GetByIdPost.Query(id);
            var result = await sender.Send(query);

            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return NotFound(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetAllPosts.Query();
            var result = await sender.Send(query);

            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return NotFound(result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet("{post_id}/comments")]
        public async Task<IActionResult> GetByPostIdAsync(int post_id)
        {
            var query = new GetByPostIdComments.Query(post_id);
            var result = await sender.Send(query);
            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return BadRequest(result.ErrorMessage);
        }
    }
}
