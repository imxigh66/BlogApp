using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Story
{
    public class StoryMediaService : IStoryMediaService
    {
        private readonly string _mediaStoragePath;

        public StoryMediaService(IConfiguration configuration)
        {
            _mediaStoragePath = configuration["StorageSettings:MediaPath"] ?? "wwwroot/stories";

            // Создаем директорию, если она не существует
            if (!Directory.Exists(_mediaStoragePath))
            {
                Directory.CreateDirectory(_mediaStoragePath);
            }
        }

        public async Task<string> UploadMediaAsync(byte[] mediaData, string fileName)
        {
            // Генерируем уникальное имя файла
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_mediaStoragePath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, mediaData);

            // Возвращаем относительный URL
            return $"/stories/{uniqueFileName}";
        }

        public Task<bool> DeleteMediaAsync(string mediaUrl)
        {
            if (string.IsNullOrEmpty(mediaUrl))
                return Task.FromResult(false);

            try
            {
                // Извлекаем имя файла из URL
                var fileName = Path.GetFileName(mediaUrl);
                var filePath = Path.Combine(_mediaStoragePath, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
