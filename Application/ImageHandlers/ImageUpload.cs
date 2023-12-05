using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ImageHandlers
{
    public class ImageUpload
    {
        public class Command : IRequest<Result<Image>>
        {
            
            public IFormFile ImageFile { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Image>>
        {
           
            private readonly IImageRepository _imageRepository;
            private readonly IAccessUser _accessUser;
            private readonly ICloudinaryService _cloudinaryService;

            public Handler( IImageRepository imageRepository, IAccessUser accessUser, ICloudinaryService cloudinaryService)
            {
                
                _imageRepository = imageRepository;
                _accessUser = accessUser;
                _cloudinaryService = cloudinaryService; 
            }

            public async Task<Result<Image>> Handle(Command request, CancellationToken cancellationToken)
            {

                var (imageUrl, PublicId) = await _cloudinaryService.UploadImageAsync(request.ImageFile);

                var userId =  _accessUser.GetUser(); 
                var image = new Image
                {
                    Url = imageUrl,
                    FileName = request.ImageFile.FileName,
                    Size = request.ImageFile.Length,
                    ContentType = request.ImageFile.ContentType,
                    ApplicationUserId= userId,
                    PublicId = PublicId
                    
                };

                
                await _imageRepository.CreateAsync(image);

                return Result<Image>.SuccessResult(image);
            }
        }
    }
}
