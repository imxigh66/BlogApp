﻿using Application.Abstractions;
using Application.Posts.Commands;
using MediatR;

namespace Application.Posts.CommandHandlers
{
    public class DeletePostHandler : IRequestHandler<DeletePost>
    {
        private readonly IPostRepository _repository;
        public DeletePostHandler(IPostRepository repository)
        {
                _repository = repository;
        }
        public async  Task Handle(DeletePost request, CancellationToken cancellationToken)
        {
            await _repository.DeletePost(request.PostId);
        }
    }
}
