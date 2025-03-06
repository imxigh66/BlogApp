

namespace Domain.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = false; // По умолчанию статья на модерации
        public int Likes { get; set; } = 0; // Количество лайков
        public List<Comment> Comments { get; set; } = new();
    }
}
