using Application.Abstractions;
using Application.Images;
using DataAccess.Storage;
using DataAccess;

namespace MinimalAPI.Exstensions
{
    public static class ImageStorageServiceExtensions
    {
        public static IServiceCollection AddImageStorage(this IServiceCollection services, IConfiguration configuration)
        {
            // Получаем тип хранилища из конфигурации
            var storageType = configuration.GetValue<string>("ImageStorage:Type");
            Console.WriteLine($"Storage type from config: '{storageType}'");

            switch (storageType?.ToLower())
            {
                case "googlecloud":
                    services.AddSingleton<IImageStorage>(provider =>
                    {
                        var projectId = configuration["ImageStorage:GoogleCloud:ProjectId"];
                        var bucketName = configuration["ImageStorage:GoogleCloud:BucketName"];
                        return new GoogleCloudStorage(projectId, bucketName);
                    });
                    break;

                case "database":
                    services.AddScoped<IImageStorage>(provider =>
                    {
                        var dbContext = provider.GetRequiredService<BlogDbContext>();
                        var baseUrl = configuration["ImageStorage:Database:BaseUrl"];
                        return new DatabaseImageStorage(dbContext, baseUrl);
                    });
                    break;

                case "local":
                default:
                    services.AddSingleton<IImageStorage>(provider =>
                    {
                        var basePath = configuration["ImageStorage:Local:BasePath"];
                        var baseUrl = configuration["ImageStorage:Local:BaseUrl"];
                        return new LocalFileStorage(basePath, baseUrl);
                    });
                    break;
            }

            services.AddScoped<IImageManager, ArticleImageManager>();

            return services;
        }
    }
}
