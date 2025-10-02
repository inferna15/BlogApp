namespace BlogApp.Domain.Entities
{
    public class Post(string title, string content, int userId, int categoryId)
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = title;
        public string Content { get; private set; } = content;
        public int UserId { get; private set; } = userId;
        public User? User { get; private set; }
        public int CategoryId { get; private set; } = categoryId;
        public Category? Category { get; private set; }
        public ICollection<Comment> Comments { get; private set; } = [];

        public void Update(string title, string content, int categoryId)
        {
            Title = title;
            Content = content;
            CategoryId = categoryId;
        }
    }
}
