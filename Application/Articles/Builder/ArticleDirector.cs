using Application.Content;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Builder
{
    public class ArticleDirector
    {
        private readonly IArticleBuilder _builder;

        public ArticleDirector(IArticleBuilder builder)
        {
            _builder = builder;
        }

        public Article CreateSimpleArticle(string title, string content, int authorId, bool isPublished = false)
        {
            return _builder
                .SetTitle(title)
                .SetContent(content)
                .SetAuthor(authorId)
                .SetIsPublished(isPublished)
                .Build();
        }

        public Article CreateArticleWithImage(string title, string content, int authorId,
            string imageUrl, string imageAlt, bool isPublished = false)
        {
            return _builder
                .SetTitle(title)
                .SetContent(content)
                .SetAuthor(authorId)
                .AddImageContent("Featured Image", imageUrl, imageAlt)
                .SetIsPublished(isPublished)
                .Build();
        }

        public Article CreateArticleWithVideo(string title, string content, int authorId,
            string videoUrl, bool isPublished = false)
        {
            return _builder
                .SetTitle(title)
                .SetContent(content)
                .SetAuthor(authorId)
                .AddVideoContent("Featured Video", videoUrl)
                .SetIsPublished(isPublished)
                .Build();
        }

        public Article CreateImageGalleryArticle(string title, string content, int authorId,
            List<(string Url, string Alt)> images, bool isPublished = false)
        {
            _builder
                .SetTitle(title)
                .SetContent(content)
                .SetAuthor(authorId)
                .SetIsPublished(isPublished);

            int i = 1;
            foreach (var (url, alt) in images)
            {
                _builder.AddImageContent($"Gallery Image {i++}", url, alt);
            }

            return _builder.Build();
        }

        public Article CreateRichMediaArticle(string title, string content, int authorId,
            List<(ContentType Type, string Title, Dictionary<string, string> Parameters)> contentItems,
            bool isPublished = false)
        {
            _builder
                .SetTitle(title)
                .SetContent(content)
                .SetAuthor(authorId)
                .SetIsPublished(isPublished);

            foreach (var item in contentItems)
            {
                var contentItem = ContentFactoryProducer.GetFactory(item.Type)
                    .CreateContent(item.Title, item.Parameters);
                _builder.AddContentItem(contentItem);
            }

            return _builder.Build();
        }
    }
}
