using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Export
{
    public class ArticleExportModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }

        public static ArticleExportModel FromArticle(Domain.Models.Article article, int commentsCount, int likesCount)
        {
            return new ArticleExportModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                AuthorName = article.Author?.Username ?? "Неизвестный автор",
                CreatedAt = article.CreatedAt,
                CommentsCount = commentsCount,
                LikesCount = likesCount
            };
        }
    }
}
