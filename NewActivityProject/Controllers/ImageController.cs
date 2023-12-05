using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.ImageHandlers;

namespace NewActivityProject.Controllers
{
    public class ImageController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator, ILogger<ImageController> logger)
            : base(logger) 
        {
            _mediator = mediator;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file provided or file is empty.");
            }

            var command = new ImageUpload.Command
            {
                ImageFile = imageFile,
                 
            };

            var result = await _mediator.Send(command);

            

            return HandleResults(result);
        }
       
    }
}
