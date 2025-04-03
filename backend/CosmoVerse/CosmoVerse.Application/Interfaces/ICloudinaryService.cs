using CosmoVerse.Application.DTOs;

namespace CosmoVerse.Infrastructure.Services
{
    public interface ICloudinaryService
    {
        Task<ImageDto> UploadImageAsync(Stream fileStream, string fileName);
    }
}