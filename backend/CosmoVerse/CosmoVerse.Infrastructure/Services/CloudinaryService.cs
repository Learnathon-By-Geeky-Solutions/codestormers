﻿
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CosmoVerse.Application.DTOs;
using Microsoft.Extensions.Configuration;

namespace CosmoVerse.Infrastructure.Services
{
    internal class CloudinaryService : ICloudinaryService
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
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
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
    }
}
