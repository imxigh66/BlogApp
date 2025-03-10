using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Likes.Command
{
    public class ToggleLikeCommand : IRequest<LikeResult>
    {
        public int ArticleId { get; set; }
        public string IpAddress { get; set; }
    }
}
