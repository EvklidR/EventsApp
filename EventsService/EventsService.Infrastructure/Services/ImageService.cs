using EventsService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using EventsService.Application.Exceptions;

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

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {

            var fileExtension = Path.GetExtension(imageFile.FileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var randomNumber = new Random().Next(1000, 9999);
            var uniqueFileName = $"img_{timestamp}_{randomNumber}{fileExtension}";
            var filePath = Path.Combine(_imagePath, uniqueFileName);

            while (File.Exists(filePath))
            {
                randomNumber = new Random().Next(1000, 9999);
                uniqueFileName = $"img_{timestamp}_{randomNumber}{fileExtension}";
                filePath = Path.Combine(_imagePath, uniqueFileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return uniqueFileName;

        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<byte[]> GetImageAsync(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new NotFoundException($"File {fileName} not found");
            }

            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

            return fileBytes;
        }

        public async Task<byte[]> GetCashedImageAsync(string fileName)
        {
            byte[] cachedFile = await _redisDb.StringGetAsync(fileName);
            if (cachedFile != null && cachedFile.Length > 0)
            {
                return cachedFile;
            }

            byte[] fileBytes = await GetImageAsync(fileName);

            await _redisDb.StringSetAsync(fileName, fileBytes, TimeSpan.FromMinutes(30));

            return fileBytes;
        }
    }
}
