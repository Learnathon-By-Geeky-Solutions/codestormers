using CosmoVerse.Application.DTOs;

namespace CosmoVerse.Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageDto> UploadImageAsync(Stream fileStream, string fileName);
        Task<bool> DeleteImageAsync(string publicId);
    }
}