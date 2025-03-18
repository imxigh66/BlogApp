using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Command
{
    public class CloneArticle : IRequest<ArticleResult>
    {
        public int SourceArticleId { get; set; }
        public bool AsDraft { get; set; } = true;
    }
}
