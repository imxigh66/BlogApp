using Application.Authorization.Command;
using MediatR;
using MinimalAPI.Abstractions;

namespace MinimalAPI.EndpointDefinitions
{
    public class AuthEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            app.MapPost("api/register", async (IMediator mediator, RegisterUserCommand command) =>
            {
                return await mediator.Send(command);
            });

            app.MapPost("api/login", async (IMediator mediator, LoginUserCommand command) =>
            {
                return await mediator.Send(command);
            });

        }
    }
}
