using BlogApp.Application.Features.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("{user_id}/posts")]
        public async Task<IActionResult> GetByAuthorIdAsync(int user_id)
        {
            var query = new GetByUserIdPosts.Query(user_id);
            var result = await sender.Send(query);

            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return NotFound(result.ErrorMessage);
        }
    }
}
