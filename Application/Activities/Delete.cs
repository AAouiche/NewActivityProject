using Domain.Models;
using Infrastructure.Repositories;
using Domain.Validation;  
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>  
        {
            public Guid Id { get; set; }
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
                
                    
                    var activity = await _activityRepository.GetByIdAsync(request.Id);
                    if (activity == null)
                    {
                        return Result<Unit>.Failure("Activity not found");
                    }

                    await _activityRepository.DeleteAsync(request.Id);
                    
                    return Result<Unit>.SuccessResult(Unit.Value); 
                
            }
        }
    }
}