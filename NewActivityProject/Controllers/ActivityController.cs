using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Activities; 
using Domain.Models;
using Domain.Validation;
using Domain.DTO;

namespace NewActivityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<Result<List<ActivityDTO>>>> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var query = new List.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return HandleResults(result);
        }
        [HttpPost("TestCreate")]
        public ActionResult<string> TestCreate()
        {
            return "Hit TestCreate";
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _mediator.Send(new Details.Query(id));
            return HandleResults(result);
        }
        [HttpPut("Edit")]
        public async Task<ActionResult<Unit>> Edit( Activity activity)
        {
            var result = await _mediator.Send(new Edit.Command {  Activity = activity });
            return HandleResults(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Activity activity)
        {
            

            var result = await _mediator.Send(new Create.Command { Activity = activity });

            return HandleResults(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return HandleResults(await _mediator.Send(new Delete.Command { Id = id }));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> GetActivity(Guid id)
        {
            return HandleResults(await _mediator.Send(new Delete.Command { Id = id }));
        }
        [HttpPut]
        /*public async Task<ActionResult<Unit>> AddAttendee( Activity activity)
        {
            return await _mediator.Send(new AddAttendee.Command {  Activity = activity });
        }*/
        [HttpPost("attending/{id}")]
        public async Task<ActionResult<Unit>> Attending(Guid id)
        {
            return HandleResults(await _mediator.Send(new updateAttendance.Command { Id = id }));
        }
    }
}