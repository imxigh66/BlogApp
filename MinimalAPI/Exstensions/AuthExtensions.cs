using Domain.Enumerations;
using System.Security.Claims;

namespace MinimalAPI.Exstensions
{
    public static class AuthExtensions
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Политика только для авторов
                options.AddPolicy("Author", policy =>
                    policy.RequireRole(UserRole.Author.ToString(), UserRole.Admin.ToString()));

                // Политика только для администраторов
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole(UserRole.Admin.ToString()));

                // Политика для авторов с высоким рейтингом (могут публиковать без модерации)
                options.AddPolicy("HighRatedAuthors", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == UserRole.Admin.ToString()) ||
                        (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == UserRole.Author.ToString()) &&
                         context.User.HasClaim(c => c.Type == "Rating" && int.Parse(c.Value) >= 50))
                    ));
            });
        }
    }
}
