using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Export
{
    public class ArticleExportService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ArticleExporterFactory _exporterFactory;

        public ArticleExportService(
            IArticleRepository articleRepository,
            ArticleExporterFactory exporterFactory)
        {
            _articleRepository = articleRepository;
            _exporterFactory = exporterFactory;
        }

        public async Task<ExportResult> ExportArticle(int articleId, ExportFormat format)
        {
            var article = await _articleRepository.GetArticleById(articleId);

            if (article == null)
            {
                return new ExportResult
                {
                    Success = false,
                    ErrorMessage = "Статья не найдена"
                };
            }

            try
            {
                var exporter = _exporterFactory.CreateExporter(format);
                var fileData = exporter.Export(article);

                return new ExportResult
                {
                    Success = true,
                    FileName = $"article_{articleId}.{exporter.GetFileExtension()}",
                    ContentType = exporter.GetContentType(),
                    FileData = fileData
                };
            }
            catch (Exception ex)
            {
                return new ExportResult
                {
                    Success = false,
                    ErrorMessage = $"Ошибка при экспорте статьи: {ex.Message}"
                };
            }
        }
    }
}
