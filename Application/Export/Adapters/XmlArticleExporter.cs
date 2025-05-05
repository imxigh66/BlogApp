using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Application.Export.Adapters
{
    public class XmlArticleExporter : IArticleExporter
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public XmlArticleExporter(ICommentRepository commentRepository, ILikeRepository likeRepository)
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
                var serializer = new XmlSerializer(typeof(ArticleExportModel));
                serializer.Serialize(memoryStream, exportModel);
                return memoryStream.ToArray();
            }
        }

        public string GetContentType()
        {
            return "application/xml";
        }

        public string GetFileExtension()
        {
            return "xml";
        }
    }
}
