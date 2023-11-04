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
            private readonly IBlobStorageService _imageService;
            private readonly IImageRepository _imageRepository;
            private readonly IAccessUser _accessUser;

            public Handler(IBlobStorageService imageService, IImageRepository imageRepository, IAccessUser accessUser)
            {
                _imageService = imageService;
                _imageRepository = imageRepository;
                _accessUser = accessUser;
            }

            public async Task<Result<Image>> Handle(Command request, CancellationToken cancellationToken)
            {

                var (imageUrl, blobName) = await _imageService.UploadImageAsync(request.ImageFile);

                var userId =  _accessUser.GetUser(); 
                var image = new Image
                {
                    Url = imageUrl,
                    FileName = request.ImageFile.FileName,
                    Size = request.ImageFile.Length,
                    ContentType = request.ImageFile.ContentType,
                    ApplicationUserId= userId,
                    CurrentBlobName= blobName
                    
                };

                
                await _imageRepository.CreateAsync(image);

                return Result<Image>.SuccessResult(image);
            }
        }
    }
}
