using Application.Abstractions;
using Application.Articles.Dto;
using Application.Articles.Queries;
using Application.Posts.Queries;
using Domain.Models;
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

                articlesDto.Add(new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    CreatedAt = article.CreatedAt,
                    AuthorName = article.Author?.Username,
                    LikesCount = likesCount,
                    CommentsCount = commentsCount
                });
            }

            return articlesDto.OrderByDescending(a => a.CreatedAt).ToList();
        }
    }
}
