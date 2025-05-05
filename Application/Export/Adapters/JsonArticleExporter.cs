using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Export.Adapters
{
    public class JsonArticleExporter : IArticleExporter
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public JsonArticleExporter(ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        public byte[] Export(Article article)
        {
            var commentsCount = _commentRepository.GetCommentCountForArticleAsync(article.Id).Result;
            var likesCount = _likeRepository.GetLikeCountForArticleAsync(article.Id).Result;

            var exportModel = ArticleExportModel.FromArticle(article, commentsCount, likesCount);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(exportModel, options);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        public string GetContentType()
        {
            return "application/json";
        }

        public string GetFileExtension()
        {
            return "json";
        }
    }
}
