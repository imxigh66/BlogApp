using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Likes.Query
{
    public class GetArticleLikesCountQuery : IRequest<int>
    {
        public int ArticleId { get; set; }
    }
}
