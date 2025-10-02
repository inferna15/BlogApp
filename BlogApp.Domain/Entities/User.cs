using Microsoft.AspNetCore.Identity;

namespace BlogApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; private set; } = [];
        public ICollection<Comment> Comments { get; private set; } = [];
    }
}
