using Application.Abstractions;
using Application.Articles.Builder;
using Application.Articles.Command;
using Application.Content;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.CommandHandler
{
    public class AddRichArticleHandler : IRequestHandler<AddRichArticle, ArticleResult>
    {
        private readonly IArticleRepository _repository;
        private readonly IAuthRepository _authRepository;
        private readonly IImageManager _imageManager;
        public AddRichArticleHandler(IArticleRepository repository, IAuthRepository authRepository, IImageManager imageManager  )
        {
            _repository = repository;
            _authRepository = authRepository;
            _imageManager = imageManager;
        }

        public async Task<ArticleResult> Handle(AddRichArticle request, CancellationToken cancellationToken)
        {
            var author = await _authRepository.GetUserByIdAsync(request.AuthorId);
            if (author == null)
            {
                return new ArticleResult { Success = false, Message = "Автор не найден" };
            }

            // Используем строителя для создания статьи
            var builder = new ArticleBuilder(_imageManager);

            // Готовим статью
            builder.SetTitle(request.Title)
                   .SetContent(request.Content)
                   .SetAuthor(request.AuthorId)
                   .SetIsPublished(!request.NeedsModeration);

            // Перебираем элементы контента и добавляем их к статье
            foreach (var item in request.ContentItems)
            {
                if (item.ContentType == ContentType.Image)
                {
                    string url = item.Parameters.GetValueOrDefault("url", "");
                    string alt = item.Parameters.GetValueOrDefault("alt", item.Title);

                    // Проверяем, является ли URL настоящим URL (не data:url)
                    if (Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == "http" || uri.Scheme == "https"))
                    {
                        try
                        {
                            using var client = new HttpClient();
                            byte[] imageData = await client.GetByteArrayAsync(uri);
                            string filename = Path.GetFileName(uri.LocalPath);

                            Console.WriteLine($"Скачано изображение: {filename}, размер: {imageData.Length}");

                            // Используем асинхронный метод для добавления контента с изображением
                            await builder.AddImageContentAsync(item.Title, imageData, filename, alt);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при загрузке изображения: {ex.Message}");
                            // В случае ошибки просто добавляем с исходным URL
                            builder.AddImageContent(item.Title, url, alt);
                        }
                    }
                    else
                    {
                        // Просто добавляем с исходным URL
                        builder.AddImageContent(item.Title, url, alt);
                    }
                }
                else
                {
                    // Для остальных типов контента используем стандартные методы
                    switch (item.ContentType)
                    {
                        case ContentType.Text:
                            builder.AddTextContent(item.Title,
                                item.Parameters.GetValueOrDefault("body", ""));
                            break;
                        case ContentType.Video:
                            builder.AddVideoContent(item.Title,
                                item.Parameters.GetValueOrDefault("url", ""));
                            break;
                        default:
                            var contentItem = ContentFactoryProducer.GetFactory(item.ContentType)
                                .CreateContent(item.Title, item.Parameters);
                            builder.AddContentItem(contentItem);
                            break;
                    }
                }
            }

            var article = builder.Build();
            var addedArticle = await _repository.AddArticle(article);

            return new ArticleResult
            {
                Success = true,
                ArticleId = addedArticle.Id,
                IsPublished = addedArticle.IsPublished,
                Message = addedArticle.IsPublished
                    ? "Статья с мультимедиа опубликована"
                    : "Статья с мультимедиа отправлена на модерацию"
            };
        }
    }
}
