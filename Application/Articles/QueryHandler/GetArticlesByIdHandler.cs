using Application.Abstractions;
using Application.Articles.Dto;
using Application.Articles.Queries;
using Application.Comments.DTO;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.QueryHandler
{
    public class GetArticlesByIdHandler : IRequestHandler<GetArticlesById, ArticleDetailDto>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;
        public GetArticlesByIdHandler(
       IArticleRepository articleRepository,
       ILikeRepository likeRepository,
       ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ArticleDetailDto> Handle(GetArticlesById request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetArticleById(request.ArticleId);

            if (article == null || !article.IsPublished)
            {
                return null;
            }

            // Получаем лайки
            var likesCount = await _likeRepository.GetLikeCountForArticleAsync(article.Id);

            // Получаем комментарии
            var comments = await _commentRepository.GetByArticleIdAsync(article.Id);
            var commentDtos = comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.PostedAt,
                AuthorName = c.Username
            }).ToList();

            return new ArticleDetailDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                AuthorName = article.Author?.Username,
                LikesCount = likesCount,
                Comments = commentDtos
            };
        }
    
    }
}
