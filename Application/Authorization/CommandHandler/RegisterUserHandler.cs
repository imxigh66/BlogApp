using Application.Abstractions;
using Application.Authorization.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.CommandHandler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResult>
    {
        private readonly IAuthRepository _authRepository;

        public RegisterUserHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _authRepository.RegisterAsync(request.Username, request.Email, request.Password, request.Role);
        }
    }

}
