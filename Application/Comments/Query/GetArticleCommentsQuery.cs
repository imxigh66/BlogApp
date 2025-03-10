using Application.Comments.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Query
{
    public class GetArticleCommentsQuery:IRequest<List<CommentDto>>
    {
        public int ArticleId { get; set; }
    }
}
