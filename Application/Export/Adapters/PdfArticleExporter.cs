using Application.Abstractions;
using Domain.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Export.Adapters
{
    public class PdfArticleExporter : IArticleExporter
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public PdfArticleExporter(ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        public byte[] Export(Article article)
        {
            var commentsCount = _commentRepository.GetCommentCountForArticleAsync(article.Id).Result;
            var likesCount = _likeRepository.GetLikeCountForArticleAsync(article.Id).Result;

            var exportModel = ArticleExportModel.FromArticle(article, commentsCount, likesCount);

            using (var memoryStream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                // Добавление заголовка
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new Paragraph(exportModel.Title, titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Добавление информации об авторе и дате
                var infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var info = new Paragraph($"Автор: {exportModel.AuthorName}\nДата публикации: {exportModel.CreatedAt:dd.MM.yyyy HH:mm}", infoFont);
                document.Add(info);

                // Добавление статистики
                var statsFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.ITALIC);
                var stats = new Paragraph($"Лайки: {exportModel.LikesCount} | Комментарии: {exportModel.CommentsCount}", statsFont);
                document.Add(stats);

                document.Add(new Paragraph("\n"));

                // Добавление содержимого статьи
                var contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var content = new Paragraph(exportModel.Content, contentFont);
                document.Add(content);

                document.Close();
                writer.Close();

                return memoryStream.ToArray();
            }
        }

        public string GetContentType()
        {
            return "application/pdf";
        }

        public string GetFileExtension()
        {
            return "pdf";
        }
    }
}
