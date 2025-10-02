namespace BlogApp.Application.Common
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Email { get; }
        string UserName { get; }
        string Role { get; }
        bool IsInRole(string roleName);
    }
}
