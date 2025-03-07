using MediatR;

namespace Application.Articles.Commands
{
    public class DeleteArticle : IRequest
    {
        public int ArticleId { get; set; }
    }
}
