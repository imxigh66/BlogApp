using MediatR;
using Domain.Models;

namespace Application.Articles.Commands
{
    public class AddArticle : IRequest<ArticleResult>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public bool NeedsModeration { get; set; }
    }
}
