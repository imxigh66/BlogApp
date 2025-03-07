using MediatR;
using Domain.Models;

namespace Application.Articles.Commands
{
    public class UpdateArticle : IRequest<Article>
    {
        public int ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
