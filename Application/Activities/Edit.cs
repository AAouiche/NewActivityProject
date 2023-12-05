using Domain.Models;
using Infrastructure.Repositories;  
using Domain.Validation;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>  
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).NotNull().WithMessage("Activity is required.");
                When(x => x.Activity != null, () =>
                {
                    RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
                });
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>  
        {
            private readonly IActivityRepository _activityRepository;

            public Handler(IActivityRepository activityRepository)
            {
                _activityRepository = activityRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken) 
            {
                var activity = await _activityRepository.GetByIdAsync(request.Activity.Id);

                if (activity == null)
                    return Result<Unit>.Failure("Could not find activity");  

                activity.Title = request.Activity.Title;
                activity.Description = request.Activity.Description;
                activity.Date = request.Activity.Date;
                activity.Venue = request.Activity.Venue;
                activity.City = request.Activity.City;
                activity.Category= request.Activity.Category;
                

                try
                {
                    await _activityRepository.UpdateAsync(activity);
                    return Result<Unit>.SuccessResult(Unit.Value); 
                }
                catch (Exception ex) 
                {
                   
                    return Result<Unit>.Failure("An error occurred while updating the activity: " + ex.Message);  
                }
            }
        }
    }
}