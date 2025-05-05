using Application.Export;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class ExportEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("api/articles/{id}/export", async (
                int id,
                [FromQuery] string format,
                ArticleExportService exportService) =>
            {
                if (!Enum.TryParse<ExportFormat>(format, true, out var exportFormat))
                {
                    return Results.BadRequest($"Неподдерживаемый формат экспорта: {format}. Поддерживаемые форматы: {string.Join(", ", Enum.GetNames<ExportFormat>())}");
                }

                var result = await exportService.ExportArticle(id, exportFormat);

                if (!result.Success)
                {
                    return Results.BadRequest(result.ErrorMessage);
                }

                return Results.File(
                    result.FileData,
                    result.ContentType,
                    result.FileName);
            });
        }
    }
}
