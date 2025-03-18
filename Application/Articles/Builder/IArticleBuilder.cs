using Domain.Models.Content;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Builder
{
    public interface IArticleBuilder
    {
        IArticleBuilder SetTitle(string title);
        IArticleBuilder SetContent(string content);
        IArticleBuilder SetAuthor(int authorId);
        IArticleBuilder SetIsPublished(bool isPublished);

        // Методы для работы с контентом
        IArticleBuilder AddContentItem(Domain.Models.Content.Content contentItem);
        IArticleBuilder AddTextContent(string title, string body);
        IArticleBuilder AddImageContent(string title, string imageUrl, string altText);
        IArticleBuilder AddVideoContent(string title, string videoUrl, int width = 640, int height = 360);

        Article Build();
    }
}
