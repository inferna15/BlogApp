namespace BlogApp.Domain.Entities
{
    public class Comment(string content, int userId, int postId)
    {
        public int Id { get; private set; }
        public string Content { get; private set; } = content;
        public int UserId { get; private set; } = userId;
        public User? User { get; private set; }
        public int PostId { get; private set; } = postId;
        public Post? Post { get; private set; }

        public void Update(string content)
        {
            Content = content;
        }
    }
}
