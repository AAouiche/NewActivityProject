using Domain.Models;
using Infrastructure.Repositories;  // Add this line to import your IActivityRepository
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
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>  // Change return type here
        {
            private readonly IActivityRepository _activityRepository;

            public Handler(IActivityRepository activityRepository)
            {
                _activityRepository = activityRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)  // Change return type here
            {
                var activity = await _activityRepository.GetByIdAsync(request.Activity.Id);

                if (activity == null)
                    return Result<Unit>.Failure("Could not find activity");  // Failure result

                activity.Title = request.Activity.Title;
                activity.Description = request.Activity.Description;
                activity.Date = request.Activity.Date;
                // ... (other properties)

                try
                {
                    await _activityRepository.UpdateAsync(activity);
                    return Result<Unit>.SuccessResult(Unit.Value);  // Success result
                }
                catch (Exception ex)  // Handle potential exceptions from the update operation
                {
                    // Log exception (consider using a logging library/dependency injection)
                    return Result<Unit>.Failure("An error occurred while updating the activity: " + ex.Message);  // Failure result
                }
            }
        }
    }
}