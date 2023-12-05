using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IImageRepository _imageRepository;
        private readonly IAccessUser _accessUser;

        
        public CloudinaryService(Cloudinary cloudinary, IImageRepository imageRepository, IAccessUser accessUser)
        {
            _accessUser= accessUser;
            _cloudinary = cloudinary;
            _imageRepository = imageRepository;
        }

        public async Task<(string imageUrl, string publicId)> UploadImageAsync(IFormFile imageFile)
        {
            var userId = _accessUser.GetUser();

            var publicId = await _imageRepository.GetCurrentPublicId(userId);
            if (string.IsNullOrEmpty(publicId))
            {
                Guid newGuid = Guid.NewGuid();
                publicId = "profile_picture_" + userId + newGuid.ToString(); 
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                PublicId = publicId,
                Overwrite = true 
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return (uploadResult.SecureUrl.AbsoluteUri, uploadResult.PublicId);
        }
    }
   }
