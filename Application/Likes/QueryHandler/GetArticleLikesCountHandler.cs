using Application.Abstractions;
using Application.Likes.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Likes.QueryHandler
{
    public class GetArticleLikesCountHandler : IRequestHandler<GetArticleLikesCountQuery, int>
    {
        private readonly ILikeRepository _likeRepository;

        public GetArticleLikesCountHandler(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<int> Handle(GetArticleLikesCountQuery request, CancellationToken cancellationToken)
        {
            return await _likeRepository.GetLikeCountForArticleAsync(request.ArticleId);
        }
    }
}
