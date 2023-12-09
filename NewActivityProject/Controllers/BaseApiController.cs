using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace NewActivityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<BaseApiController> _logger;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;
        }

        protected ActionResult HandleResults<T>(Result<T> result)
        {
            if (result.Error == "Unauthorized")
            {
                _logger.LogWarning("HandleResults - Unauthorized access attempt.");
                return Unauthorized(new { Error = result.Error });
            }
            else if (result.Success &&result.Value == null)
            {
                string errorMessage = string.IsNullOrEmpty(result.Error) ? "Data not found" : result.Error;
                _logger.LogWarning("HandleResults - Non-successful result: {ErrorMessage}", errorMessage);
                return NotFound(new { Error = errorMessage });
            }
            else if (result.Success && result.Value != null)
            {
                _logger.LogInformation("HandleResults - Successful result");
                return Ok(result.Value);
            }
            return BadRequest(result.Error);

                
        }
    }
}