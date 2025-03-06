using Application.Abstractions;
using Application.Posts.Commands;
using DataAccess.Repositories;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Abstractions;

namespace MinimalAPI.Exstensions
{
    public static class MinimalApiExtensions
    {

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BlogDbContext>(opt => opt.UseSqlServer(cs));
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePost).Assembly));
        }

        public static void RegisterEndpointDefinitions(this WebApplication app)
        {
            var endpointDefinitions = typeof(Program).Assembly.GetTypes()
                .Where(t=>t.IsAssignableTo(typeof(IEndpointDefinition))&& !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>();

            foreach(var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.RegisterEndpoints(app);
            }
        }
    }
}
