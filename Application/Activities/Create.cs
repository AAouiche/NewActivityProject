using Domain.Models;

using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<ActivityDTO>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<ActivityDTO>>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IAccessUser _accessUser;
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IActivityRepository activityRepository, IAccessUser accessUser, AppDbContext context, IMapper mapper)
            {
                _activityRepository = activityRepository;
                _accessUser = accessUser;
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ActivityDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Activity == null)
                {
                    return Result<ActivityDTO>.Failure("Activity instance cannot be null.");
                }

                // Associate the activity with the user
                var userId = _accessUser.GetUser();
                if (string.IsNullOrEmpty(userId))
                {
                    return Result<ActivityDTO>.Failure("User is not authenticated");
                }

                // Fetch the ApplicationUser from the database based on userId
                var applicationUser = await _context.Users.FindAsync(userId);
                if (applicationUser == null)
                {
                    return Result<ActivityDTO>.Failure("User not found");
                }

                // Add the user to attendees and set them as a host
                var attendee = new ActivityAttendee
                {
                    ApplicationUserId = userId,
                    ApplicationUser = applicationUser, // Set the fetched user
                    IsHost = true,
                    Activity = request.Activity,
                };
                request.Activity.Attendees.Add(attendee);

                await _activityRepository.AddAsync(request.Activity);

                var activityDTO = _mapper.Map<ActivityDTO>(request.Activity);

                // Depending on your needs, you might return the DTO or the entity
                return Result<ActivityDTO>.SuccessResult(activityDTO);
            }
        }
    }
}