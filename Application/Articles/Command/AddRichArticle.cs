using Application.Articles.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Articles.Command
{
    public class AddRichArticle : IRequest<ArticleResult>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public bool NeedsModeration { get; set; }

        public List<ContentItemDto> ContentItems { get; set; } = new List<ContentItemDto>();
    }
}
