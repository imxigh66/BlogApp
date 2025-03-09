using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles
{
    public class ArticleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? ArticleId { get; set; }
        public bool IsPublished { get; set; }
    }
}
