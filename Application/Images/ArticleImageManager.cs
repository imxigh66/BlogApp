using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Images
{
    public class ArticleImageManager : IImageManager
    {
        private readonly IImageStorage _storage;
        private readonly string _folderPrefix = "articles";

        public ArticleImageManager(IImageStorage storage)
        {
            _storage = storage;
        }

        public async Task<string> StoreImageAsync(byte[] imageData, string filename, string altText)
        {
            Console.WriteLine($"StoreImageAsync вызван. Размер данных: {imageData?.Length ?? 0}, Имя файла: {filename}");
            // Создаем уникальное имя файла с сохранением расширения
            string extension = Path.GetExtension(filename);
            string uniqueFilename = $"{_folderPrefix}/{DateTime.UtcNow:yyyyMMdd}/{Guid.NewGuid()}{extension}";

            // Сохраняем изображение в хранилище
            string identifier = await _storage.SaveAsync(imageData, uniqueFilename);

            // Возвращаем URL или идентификатор изображения
            return _storage.GetPublicUrl(identifier);
        }

        public async Task<bool> DeleteImageAsync(string imageIdentifier)
        {
            return await _storage.DeleteAsync(imageIdentifier);
        }

        public string GetImageUrl(string imageIdentifier)
        {
            return _storage.GetPublicUrl(imageIdentifier);
        }
    }
}
