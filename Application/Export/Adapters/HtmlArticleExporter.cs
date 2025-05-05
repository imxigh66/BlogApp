using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Export.Adapters
{
    public class HtmlArticleExporter : IArticleExporter
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public HtmlArticleExporter(ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        public byte[] Export(Article article)
        {
            var commentsCount = _commentRepository.GetCommentCountForArticleAsync(article.Id).Result;
            var likesCount = _likeRepository.GetLikeCountForArticleAsync(article.Id).Result;

            var exportModel = ArticleExportModel.FromArticle(article, commentsCount, likesCount);

            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang=\"ru\">");
            html.AppendLine("<head>");
            html.AppendLine("  <meta charset=\"UTF-8\">");
            html.AppendLine($"  <title>{exportModel.Title}</title>");
            html.AppendLine("  <style>");
            html.AppendLine("    body { font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; }");
            html.AppendLine("    .article-title { text-align: center; margin-bottom: 20px; }");
            html.AppendLine("    .article-meta { color: #666; font-size: 0.9em; margin-bottom: 20px; }");
            html.AppendLine("    .article-content { margin-top: 30px; }");
            html.AppendLine("  </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine($"  <h1 class=\"article-title\">{exportModel.Title}</h1>");
            html.AppendLine("  <div class=\"article-meta\">");
            html.AppendLine($"    <div>Автор: {exportModel.AuthorName}</div>");
            html.AppendLine($"    <div>Дата публикации: {exportModel.CreatedAt:dd.MM.yyyy HH:mm}</div>");
            html.AppendLine($"    <div>Лайки: {exportModel.LikesCount} | Комментарии: {exportModel.CommentsCount}</div>");
            html.AppendLine("  </div>");

            // Преобразуем простой текст в HTML с учетом параграфов
            var contentHtml = exportModel.Content
                .Replace("\r\n", "\n")
                .Replace("\n\n", "</p><p>")
                .Replace("\n", "<br>");

            html.AppendLine($"  <div class=\"article-content\"><p>{contentHtml}</p></div>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return Encoding.UTF8.GetBytes(html.ToString());
        }

        public string GetContentType()
        {
            return "text/html";
        }

        public string GetFileExtension()
        {
            return "html";
        }
    }
}
