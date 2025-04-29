using Application.Abstractions;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Storage
{
    public class GoogleCloudStorage : IImageStorage
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public GoogleCloudStorage(string projectId, string bucketName)
        {
            _bucketName = bucketName;
            _storageClient = StorageClient.Create();
        }

        public async Task<string> SaveAsync(byte[] imageData, string filename)
        {
            using var memoryStream = new MemoryStream(imageData);

            // Загружаем файл в Google Cloud Storage
            var object_ = await _storageClient.UploadObjectAsync(
                _bucketName,
                filename,
                GetContentType(filename),
                memoryStream);

            return filename; // Возвращаем путь к файлу в бакете как идентификатор
        }

        public async Task<bool> DeleteAsync(string identifier)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, identifier);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<byte[]> GetAsync(string identifier)
        {
            try
            {
                var storageObject = await _storageClient.GetObjectAsync(_bucketName, identifier);
                using var memoryStream = new MemoryStream();
                await _storageClient.DownloadObjectAsync(_bucketName, identifier, memoryStream);
                return memoryStream.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public string GetPublicUrl(string identifier)
        {
            // Формируем публичный URL для доступа к изображению
            return $"https://storage.googleapis.com/{_bucketName}/{identifier}";
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
