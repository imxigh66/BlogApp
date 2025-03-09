using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int AuthorRating { get; set; }
    }
}
