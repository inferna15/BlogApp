using BlogApp.Application.Common;
using BlogApp.Application.Features.Comments;
using BlogApp.Presentation.Dtos.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Presentation.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController(ISender sender, ICurrentUserService service) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCommentDto dto)
        {
            var command = new CreateComment.Command(dto.PostId, service.UserId, dto.Content);
            var result = await sender.Send(command);
            if (result.IsSuccess)
                return Created();
            else
                return BadRequest(result.ErrorMessage);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCommentDto dto)
        {
            var command = new UpdateComment.Command(id, service.UserId, dto.Content);
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
            var command = new DeleteComment.Command(id, service.UserId);
            var result = await sender.Send(command);
            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }

        
    }
}
