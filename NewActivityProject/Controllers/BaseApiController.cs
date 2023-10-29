using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewActivityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices
            .GetService<IMediator>();

        protected ActionResult HandleResults<T>(Result<T> result)
        {
            if (!result.Success || result.Value == null)
            {
                string errorMessage = string.IsNullOrEmpty(result.Error)
                    ? "Data not found"
                    : result.Error;
                // Log error if needed
                return NotFound(new { Error = errorMessage });
            }

            return Ok(result.Value);

        }
    }
}
