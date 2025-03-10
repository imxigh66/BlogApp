using Application.Abstractions;
using Application.Comments.DTO;
using Application.Comments.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.QueriesHandler
{
    public class GetArticleCommentsHandler : IRequestHandler<GetArticleCommentsQuery, List<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetArticleCommentsHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDto>> Handle(GetArticleCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByArticleIdAsync(request.ArticleId);

            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.PostedAt,
                AuthorName = c.Username
            }).ToList();
        }
    }
}
