using Application.Abstractions;
using Application.Export.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Export
{
    public class ArticleExporterFactory
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public ArticleExporterFactory(ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        public IArticleExporter CreateExporter(ExportFormat format)
        {
            return format switch
            {
                ExportFormat.Pdf => new PdfArticleExporter(_commentRepository, _likeRepository),
                ExportFormat.Json => new JsonArticleExporter(_commentRepository, _likeRepository),
                ExportFormat.Xml => new XmlArticleExporter(_commentRepository, _likeRepository),
                ExportFormat.Html => new HtmlArticleExporter(_commentRepository, _likeRepository),
                _ => throw new ArgumentException($"Неподдерживаемый формат экспорта: {format}")
            };
        }
    }
}
