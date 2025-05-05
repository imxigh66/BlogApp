using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public StoryType Type { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } // Истечение через 24 часа
        public List<StoryView> Views { get; set; } = new List<StoryView>();
        public bool IsActive => DateTime.UtcNow < ExpiresAt;
    }
}
