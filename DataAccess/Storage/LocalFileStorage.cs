using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Storage
{
    public class LocalFileStorage : IImageStorage
    {
        private readonly string _basePath;
        private readonly string _baseUrl;

        public LocalFileStorage(string basePath, string baseUrl)
        {
            _basePath = basePath;
            _baseUrl = baseUrl;
        }

        public async Task<string> SaveAsync(byte[] imageData, string filename)
        {
            try
            {
                Console.WriteLine($"Сохранение изображения по пути: {_basePath}/{filename}");
                string fullPath = Path.Combine(_basePath, filename);
            string directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllBytesAsync(fullPath, imageData);
            return filename; // Возвращаем относительный путь как идентификатор
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string identifier)
        {
            try
            {
                string fullPath = Path.Combine(_basePath, identifier);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<byte[]> GetAsync(string identifier)
        {
            string fullPath = Path.Combine(_basePath, identifier);
            if (File.Exists(fullPath))
                return await File.ReadAllBytesAsync(fullPath);
            return null;
        }

        public string GetPublicUrl(string identifier)
        {
            return $"{_baseUrl}/{identifier.Replace("\\", "/")}";
        }
    }
}
