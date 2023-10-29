using Domain.Models;
using Infrastructure.Repositories;

using Domain.Validation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;


public class Details
{
    public class Query : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }

        public Query(Guid id)
        {
            Id = id;
        }
    }
    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id must not be empty");
        }
    }
    public class Handler : IRequestHandler<Query, Result<Activity>>
    {
       
        private readonly IAccessUser _accessUser;
        private readonly IActivityRepository _activityRepository;
        public Handler(IActivityRepository activityRepository, IAccessUser accessUser)
{
    _activityRepository = activityRepository;
    _accessUser = accessUser;
}

        public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            
            var activity = await _activityRepository.GetByIdWithAttendeesAsync(request.Id);

            return Result<Activity>.SuccessResult(activity);
        }
    }
}