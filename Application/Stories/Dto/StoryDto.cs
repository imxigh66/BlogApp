using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Stories.Dto
{
    public class StoryDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public string Type { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int ViewsCount { get; set; }
        public bool HasBeenViewedByCurrentUser { get; set; }
    }
}
