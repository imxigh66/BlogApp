using Application.Content;
using Domain.Models.Content;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Builder
{
    public class ArticleBuilder : IArticleBuilder
    {
        private readonly Article _article = new Article();
        private readonly ContentService _contentService = new ContentService();

        public IArticleBuilder SetTitle(string title)
        {
            _article.Title = title;
            return this;
        }

        public IArticleBuilder SetContent(string content)
        {
            _article.Content = content;
            return this;
        }

        public IArticleBuilder SetAuthor(int authorId)
        {
            _article.AuthorId = authorId;
            return this;
        }

        public IArticleBuilder SetIsPublished(bool isPublished)
        {
            _article.IsPublished = isPublished;
            return this;
        }

        public IArticleBuilder AddContentItem(Domain.Models.Content.Content contentItem)
        {
            _article.ContentItems.Add(contentItem);
            return this;
        }

        public IArticleBuilder AddTextContent(string title, string body)
        {
            var parameters = new Dictionary<string, string> { ["body"] = body };
            var content = _contentService.CreateContent(ContentType.Text, title, parameters);
            return AddContentItem(content);
        }

        public IArticleBuilder AddImageContent(string title, string imageUrl, string altText)
        {
            var parameters = new Dictionary<string, string>
            {
                ["url"] = imageUrl,
                ["alt"] = altText
            };
            var content = _contentService.CreateContent(ContentType.Image, title, parameters);
            return AddContentItem(content);
        }

        public IArticleBuilder AddVideoContent(string title, string videoUrl, int width = 640, int height = 360)
        {
            var parameters = new Dictionary<string, string>
            {
                ["url"] = videoUrl,
                ["width"] = width.ToString(),
                ["height"] = height.ToString()
            };
            var content = _contentService.CreateContent(ContentType.Video, title, parameters);
            return AddContentItem(content);
        }

        public Article Build()
        {
            _article.CreatedAt = DateTime.UtcNow;
            return _article;
        }
    }
}
