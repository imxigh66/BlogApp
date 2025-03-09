using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Command
{
    public class ModerateArticleCommand:IRequest<ArticleResult>
    {

        public int ArticleId { get; set; }
        public bool Approve { get; set; }
        public string RejectionReason { get; set; }
    }
}
