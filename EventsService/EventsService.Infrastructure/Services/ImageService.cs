using EventsService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace EventsService.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imagePath;
        private readonly IDatabase _redisDb;

        public ImageService(string imagePath, IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
            _imagePath = imagePath;
        }

        public async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(_imagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }

        public async Task<byte[]> GetImageAsync(string fileName)
        {
            byte[] cachedFile = await _redisDb.StringGetAsync(fileName);
            if (cachedFile != null && cachedFile.Length > 0)
            {
                return cachedFile;
            }

            var filePath = Path.Combine(_imagePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", fileName);
            }

            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

            await _redisDb.StringSetAsync(fileName, fileBytes, TimeSpan.FromMinutes(30));

            return fileBytes;
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
