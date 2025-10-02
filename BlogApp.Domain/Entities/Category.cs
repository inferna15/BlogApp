namespace BlogApp.Domain.Entities
{
    public class Category(string name)
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = name;
        public ICollection<Post> Posts { get; private set; } = [];

        public void Update(string name)
        {
            Name = name;
        }
    }
}
