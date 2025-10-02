using BlogApp.Application.Features.Users;
using BlogApp.Presentation.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(ISender sender) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var command = new LoginUser.Command(dto.Email, dto.Password);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Ok(result.Data);
            else 
                return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var command = new RegisterUser.Command(dto.UserName, dto.Email, dto.Password);
            var result = await sender.Send(command);

            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.ErrorMessage);
        }
    }
}
