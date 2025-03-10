using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Command
{
    public class CreateCommentCommand:IRequest<CommentResult>
    {

        public int ArticleId { get; set; }
        public string Content { get; set; }
        public string AnonymousName { get; set; }
    }
}
