using Domain.Models;
using Infrastructure.Repositories;
using Domain.Validation;  // Ensure your Result<T> class is accessible
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>  // Note the return type change to Result<Unit>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>  // Note the return type change to Result<Unit>
        {
            private readonly IActivityRepository _activityRepository;

            public Handler(IActivityRepository activityRepository)
            {
                _activityRepository = activityRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)  // Note the return type change to Result<Unit>
            {
                try
                {
                    // You might want to check if the entity exists before attempting to delete it
                    // to provide a more specific error message if it does not exist.
                    var activity = await _activityRepository.GetByIdAsync(request.Id);
                    if (activity == null)
                    {
                        return Result<Unit>.Failure("Activity not found");
                    }

                    await _activityRepository.DeleteAsync(request.Id);
                    
                    return Result<Unit>.SuccessResult(Unit.Value); // Successful deletion returns Result<Unit> with Success = true
                }
                catch (Exception ex)
                {
                    
                    return Result<Unit>.Failure($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}