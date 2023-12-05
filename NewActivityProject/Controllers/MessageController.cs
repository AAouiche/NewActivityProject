using Application.Messages;
using Domain.DTO;
using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewActivityProject.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMediator _mediator;

        
        public MessagesController(IMediator mediator, ILogger<MessagesController> logger)
            : base(logger) 
        {
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<Result<MessageDTO>>> Create(Create.Command command)
        {
            var result = await _mediator.Send(command);
            return HandleResults(result);
        }

       
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Result<Unit>>> Delete(int id)
        {
            var command = new Delete.Command { Id = id };
            var result = await _mediator.Send(command);
            return HandleResults(result);
        }
        [HttpGet]
        public async Task<ActionResult<Result<MessageDTO>>> List(Guid id)
        {
            var command = new List.Query { Id = id };
            var result = await _mediator.Send(command);
            return HandleResults(result);
        }
           


        
    }
}
