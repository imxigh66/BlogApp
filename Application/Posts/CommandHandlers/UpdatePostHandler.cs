using Application.Abstractions;
using Application.Posts.Commands;
using Domain.Models;
using MediatR;


namespace Application.Posts.CommandHandlers
{
    public class UpdatePostHandler : IRequestHandler<UpdatePost, Post>
    {
        private readonly IPostRepository _repository;
        public UpdatePostHandler(IPostRepository repository)
        {
                _repository = repository;
        }
        public async Task<Post> Handle(UpdatePost request, CancellationToken cancellationToken)
        {
            var post = await _repository.UpdatePost(request.PostContent, request.PostId);
            return post;
        }
    }
}
