using Application.Abstractions;
using Domain.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Storage
{
    public class DatabaseImageStorage : IImageStorage
    {
        private readonly BlogDbContext _dbContext;
        private readonly string _baseUrl;

        public DatabaseImageStorage(BlogDbContext dbContext, string baseUrl)
        {
            _dbContext = dbContext;
            _baseUrl = baseUrl;
        }

        public async Task<string> SaveAsync(byte[] imageData, string filename)
        {
            var image = new ImageEntity
            {
                Id = Guid.NewGuid().ToString(),
                FileName = filename,
                ContentType = GetContentType(filename),
                Data = imageData,
                UploadedAt = DateTime.UtcNow
            };

            _dbContext.Images.Add(image);
            await _dbContext.SaveChangesAsync();
            return image.Id; // Возвращаем ID как идентификатор
        }

        public async Task<bool> DeleteAsync(string identifier)
        {
            var image = await _dbContext.Images.FindAsync(identifier);
            if (image != null)
            {
                _dbContext.Images.Remove(image);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<byte[]> GetAsync(string identifier)
        {
            var image = await _dbContext.Images.FindAsync(identifier);
            return image?.Data;
        }

        public string GetPublicUrl(string identifier)
        {
            // Тут мы генерируем URL для API-эндпоинта, который будет обслуживать изображения из БД
            return $"{_baseUrl}/api/images/{identifier}";
        }

        private string GetContentType(string filename)
        {
            string extension = Path.GetExtension(filename).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
