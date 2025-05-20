using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Command
{
    public class ChangeArticleStateCommand : IRequest<ArticleResult>
    {
        public int ArticleId { get; set; }
        public string Action { get; set; } // "SendToModeration", "Draft"
    }
}
