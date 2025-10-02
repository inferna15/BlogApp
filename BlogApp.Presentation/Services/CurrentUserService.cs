using BlogApp.Application.Common;
using System.Security.Claims;

namespace BlogApp.Presentation.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => int.Parse(User?.FindFirstValue(ClaimTypes.NameIdentifier));

        public string Email => User?.FindFirstValue(ClaimTypes.Email);

        public string UserName => User?.FindFirstValue(ClaimTypes.Name);

        public string Role => User?.FindFirstValue(ClaimTypes.Role);

        public bool IsInRole(string roleName) => User?.IsInRole(roleName) ?? false;
    }
}
