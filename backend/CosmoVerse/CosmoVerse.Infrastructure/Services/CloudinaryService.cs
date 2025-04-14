
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CosmoVerse.Application.DTOs;
using Microsoft.Extensions.Configuration;

namespace CosmoVerse.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration _configuration)
        {
            var cloudinary = new Cloudinary(new Account(
                _configuration["Cloudinary:CloudName"],
                _configuration["Cloudinary:ApiKey"],
                _configuration["Cloudinary:ApiSecret"]
            ));
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Uploads an image to Cloudinary and returns the image information.
        /// </summary>
        /// <param name="fileStream">file</param>
        /// <param name="fileName">file name</param>
        /// <returns>image info</returns>
        public async Task<ImageDto> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            var imageInfo = new ImageDto
            {
                ImageUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId,
                CreatedAt = DateTime.UtcNow
            };
            return imageInfo;
        }

        /// <summary>
        /// Deletes an image from Cloudinary using its public ID.
        /// </summary>
        /// <param name="publicId">public id</param>
        /// <returns></returns>
        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);
            return deletionResult.Result == "ok";
        }
    }
}
