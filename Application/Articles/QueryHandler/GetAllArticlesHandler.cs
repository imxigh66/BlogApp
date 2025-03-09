﻿using Application.Abstractions;
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
        private readonly IArticleRepository _repository;
        public GetAllArticlesHandler(IArticleRepository repository)
        {
            _repository = repository;
        }
      
        public async Task<List<ArticleDto>> Handle(GetAllArticles request, CancellationToken cancellationToken)
        {
            var articles = await _repository.GetAllArticle();
            return articles.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedAt = a.CreatedAt,
                AuthorName = a.Author.Username,
                AuthorId = a.AuthorId,
                AuthorRating = a.Author.Rating
            }).ToList();
        }
    }
}
