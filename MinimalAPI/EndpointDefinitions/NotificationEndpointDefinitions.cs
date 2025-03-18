using Application.Notifications.Commands;
using Application.Notifications.Queries;
using MediatR;
using MinimalAPI.Abstractions;
using System.Security.Claims;

namespace MinimalAPI.EndpointDefinitions
{
    public class NotificationEndpointDefinitions : IEndpointDefinition
    {
        public void RegisterEndpoints(WebApplication app)
        {
            // Получение уведомлений текущего пользователя
            app.MapGet("api/notifications", async (
                IMediator mediator,
                HttpContext context) =>
            {
                var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var notifications = await mediator.Send(new GetUserNotifications
                {
                    UserId = userId
                });

                return notifications;
            }).RequireAuthorization("Author"); 

            // Пометка уведомления как прочитанное
            app.MapPost("api/notifications/{id}/read", async (
                int id,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new MarkNotificationAsRead
                {
                    NotificationId = id
                });

                return result ? Results.Ok() : Results.NotFound();
            }).RequireAuthorization("Author");
        }
    }
}
