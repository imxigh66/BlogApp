using Application.Abstractions;
using Application.Likes.Command;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Likes.CommandHandler
{
    public class ToggleLikeHandler : IRequestHandler<ToggleLikeCommand, LikeResult>
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IAuthRepository _authRepository;

        public ToggleLikeHandler(
            ILikeRepository likeRepository,
            IArticleRepository articleRepository,
            IAuthRepository authRepository)
        {
            _likeRepository = likeRepository;
            _articleRepository = articleRepository;
            _authRepository = authRepository;
        }

        public async Task<LikeResult> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetArticleById(request.ArticleId);
            if (article == null || !article.IsPublished)
            {
                return new LikeResult { Success = false, Message = "Статья не найдена или не опубликована" };
            }

            // Проверяем, есть ли уже лайк с этого IP
            var existingLike = await _likeRepository.GetByArticleAndIpAsync(request.ArticleId, request.IpAddress);

            if (existingLike != null)
            {
                // Удаляем лайк (toggle)
                await _likeRepository.DeleteAsync(existingLike.Id);
                return new LikeResult { Success = true, Message = "Лайк удален", IsLiked = false };
            }

            // Добавляем лайк
            var like = new Like
            {
                ArticleId = request.ArticleId,
                ClientIpAddress = request.IpAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _likeRepository.AddAsync(like);

            // Получаем автора и увеличиваем рейтинг
            var author = await _authRepository.GetUserByIdAsync(article.AuthorId);
            if (author != null)
            {
                author.Rating++;
                await _authRepository.UpdateUserAsync(author);
            }

            return new LikeResult { Success = true, Message = "Лайк добавлен", IsLiked = true };
        }
    }
}
