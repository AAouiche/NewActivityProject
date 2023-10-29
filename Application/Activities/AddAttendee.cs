
using Domain.Models;
using Infrastructure.Repositories;

using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Activities
{
    public class AddAttendee
    {
        /*public class Command : IRequest<Unit>
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IActivityAttendeeRepository _attendeeRepo;
            private readonly IAccessUser _accessUser;

            public Handler(IActivityAttendeeRepository attendeeRepo, IAccessUser accessUser)
            {
                _attendeeRepo = attendeeRepo;
                _accessUser = accessUser;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var userId = _accessUser.GetUser();
                var newAttendee = new ActivityAttendee
                {
                    ActivityId = request.Activity.Id,
                    ApplicationUserId = userId,
                    IsHost = false
                };

                await _attendeeRepo.AddAsync(newAttendee);
                await _attendeeRepo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }*/
    }
}