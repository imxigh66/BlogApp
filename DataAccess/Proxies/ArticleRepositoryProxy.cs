using Application.Abstractions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Proxies
{
    public class ArticleRepositoryProxy : IArticleRepository
    {
        private readonly IArticleRepository _repository;
        private readonly ILogger _logger;

        public ArticleRepositoryProxy(IArticleRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Article> AddArticle(Article article)
        {
            _logger.LogInfo($"Добавление новой статьи: '{article.Title}' от автора с ID: {article.AuthorId}");
            try
            {
                var result = await _repository.AddArticle(article);
                _logger.LogInfo($"Статья успешно добавлена с ID: {result.Id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении статьи: '{article.Title}'", ex);
                throw;
            }
        }

        public async Task DeleteArticle(int id)
        {
            _logger.LogInfo($"Удаление статьи с ID: {id}");
            try
            {
                await _repository.DeleteArticle(id);
                _logger.LogInfo($"Статья с ID: {id} успешно удалена");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении статьи с ID: {id}", ex);
                throw;
            }
        }

        public async Task<ICollection<Article>> GetAllArticle()
        {
            _logger.LogInfo("Запрос на получение всех статей");
            try
            {
                var articles = await _repository.GetAllArticle();
                _logger.LogInfo($"Получено {articles.Count} статей");
                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при получении всех статей", ex);
                throw;
            }
        }

        public async Task<Article> GetArticleById(int id)
        {
            _logger.LogInfo($"Запрос статьи по ID: {id}");
            try
            {
                var article = await _repository.GetArticleById(id);
                if (article != null)
                    _logger.LogInfo($"Статья с ID: {id} найдена");
                else
                    _logger.LogWarning($"Статья с ID: {id} не найдена");
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при получении статьи с ID: {id}", ex);
                throw;
            }
        }

        public async Task<ICollection<Article>> GetPendingArticles()
        {
            _logger.LogInfo("Запрос на получение статей на модерации");
            try
            {
                var articles = await _repository.GetPendingArticles();
                _logger.LogInfo($"Получено {articles.Count} статей на модерации");
                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при получении статей на модерации", ex);
                throw;
            }
        }

        public async Task<Article> UpdateArticle(string updatedContent, int articleId)
        {
            _logger.LogInfo($"Обновление содержимого статьи с ID: {articleId}");
            try
            {
                var article = await _repository.UpdateArticle(updatedContent, articleId);
                _logger.LogInfo($"Статья с ID: {articleId} успешно обновлена");
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обновлении статьи с ID: {articleId}", ex);
                throw;
            }
        }

        public async Task UpdateArticleStatus(int articleId, bool isPublished)
        {
            _logger.LogInfo($"Изменение статуса статьи с ID: {articleId} на {(isPublished ? "опубликовано" : "на модерации")}");
            try
            {
                await _repository.UpdateArticleStatus(articleId, isPublished);
                _logger.LogInfo($"Статус статьи с ID: {articleId} успешно изменен");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при изменении статуса статьи с ID: {articleId}", ex);
                throw;
            }
        }
    }
}
