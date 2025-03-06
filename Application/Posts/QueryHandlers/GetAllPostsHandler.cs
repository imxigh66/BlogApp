using Application.Abstractions;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;


namespace Application.Posts.QueryHandlers
{
    public class GetAllPostsHandler : IRequestHandler<GetAllPosts, ICollection<Post>>
    {
        private readonly IPostRepository _repository;
        public GetAllPostsHandler(IPostRepository repository)
        {
            _repository = repository;
        }
        public async Task<ICollection<Post>> Handle(GetAllPosts request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllPosts();
        }
    }
}
