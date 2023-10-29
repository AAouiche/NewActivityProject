﻿using Domain.DTO;
using Domain.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Repositories;
using Infrastructure.Security;
using Domain.Interfaces;

namespace Application.Activities
{
    public class updateAttendance
    {
        
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context; // Replace DbContext with your specific _context type
            private readonly IAccessUser _accessUser; // Assuming you have an interface for user accessor
            private readonly IActivityRepository _activityRepository;


            public Handler(AppDbContext context, IAccessUser accessUser,IActivityRepository activityRepository) // Constructor injection
            {
                _context = context;
                _accessUser = accessUser;
                _activityRepository = activityRepository;
                
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _activityRepository.GetActivityWithAttendees(request.Id);

                if (activity == null) return null;


                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _accessUser.GetUser());

                if (user == null) return null;

                var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.ApplicationUser.UserName;
                var attendance = activity.Attendees.FirstOrDefault(x => x.ApplicationUser.UserName == user.UserName);

                if (attendance != null && hostUsername == user.UserName)
                {
                    activity.cancelled = !activity.cancelled;
                }

                if (attendance != null && hostUsername != user.UserName)
                {
                    activity.Attendees.Remove(attendance);
                }

                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        ApplicationUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }
                var result = await _context.SaveChangesAsync()>0;
                return result ? Result<Unit>.SuccessResult(Unit.Value): Result<Unit>.Failure("something went wrong");
            }
        }
        




    }
}
