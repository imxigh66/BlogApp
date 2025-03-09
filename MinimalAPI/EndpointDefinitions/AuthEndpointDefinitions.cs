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
                var result = await mediator.Send(command);

                if (result.Success)
                {
                    return Results.Ok(result);
                }

                return Results.BadRequest(result);
            }).AllowAnonymous();

            app.MapPost("api/login", async (IMediator mediator, LoginUserCommand command) =>
            {
                var result = await mediator.Send(command);

                if (result.Success)
                {
                    return Results.Ok(result);
                }

                return Results.Unauthorized();
            }).AllowAnonymous();

        }


    }
}
