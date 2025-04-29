using Application.Abstractions;
using Application.Articles.Dto;
using Application.Articles.Queries;
using Application.Content;
using Application.Posts.Queries;
using Domain.Models;
using Domain.Models.Content;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.QueryHandler
{
    public class GetAllArticlesHandler : IRequestHandler<GetAllArticles, List<ArticleDto>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;
        public GetAllArticlesHandler(
      IArticleRepository articleRepository,
      ILikeRepository likeRepository,
      ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
        }

        public async Task<List<ArticleDto>> Handle(GetAllArticles request, CancellationToken cancellationToken)
        {
            var articles = await _articleRepository.GetAllArticle();
            var publishedArticles = articles.Where(a => a.IsPublished).ToList();

            // Создаем список DTO с подсчетом лайков и комментариев
            var articlesDto = new List<ArticleDto>();

            foreach (var article in publishedArticles)
            {
                var likesCount = await _likeRepository.GetLikeCountForArticleAsync(article.Id);
                var commentsCount = await _commentRepository.GetCommentCountForArticleAsync(article.Id);

                // Создаем список контентных элементов
                var contentItems = article.ContentItems?
                    .Select(c => new ContentItemDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        ContentType = Enum.TryParse<ContentType>(c.ContentType, out var contentType)
            ? contentType
            : ContentType.Text, // Значение по умолчанию, если парсинг не удался
                        // Специфичные свойства в зависимости от типа
                        ImageUrl = c is ImageContent img ? img.ImageUrl : null,
                        AltText = c is ImageContent imgAlt ? imgAlt.AltText : null,
                        Body = c is TextContent txt ? txt.Body : null,
                        VideoUrl = c is VideoContent vid ? vid.VideoUrl : null,
                        Width = c is VideoContent vidW ? vidW.Width : null,
                        Height = c is VideoContent vidH ? vidH.Height : null
                    })
                    .ToList() ?? new List<ContentItemDto>();

                articlesDto.Add(new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,  // Добавляем текстовый контент
                    CreatedAt = article.CreatedAt,
                    AuthorName = article.Author?.Username,
                    AuthorId = article.AuthorId,
                    AuthorRating = article.Author?.Rating ?? 0,
                    LikesCount = likesCount,
                    CommentsCount = commentsCount,
                    ContentItems = contentItems  // Добавляем список контентных элементов
                });
            }

            return articlesDto.OrderByDescending(a => a.CreatedAt).ToList();
        }
    }
}
