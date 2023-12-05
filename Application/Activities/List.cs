using Domain.DTO;
using Domain.Models;
using Infrastructure.Repositories;

using Domain.Validation;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;



namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PaginatedResult<ActivityDTO>>>
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            
            public string Filter { get; set; } = "all";
            public DateTime? SelectedDate { get; set; } 
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedResult<ActivityDTO>>>
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IAccessUser _accessUser;
            private readonly IMapper _mapper;

            public Handler(IActivityRepository activityRepository, IAccessUser accessUser, IMapper mapper)
            {
                _activityRepository = activityRepository;
                _accessUser = accessUser;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedResult<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var pagedActivities = await _activityRepository.GetAllAsync(
                    request.PageNumber,
                    request.PageSize,

                    request.Filter,
                    request.SelectedDate
                );

                var activitiesDto = _mapper.Map<List<ActivityDTO>>(pagedActivities.Items);

                var paginatedResult = new PaginatedResult<ActivityDTO>
                {
                    Items = activitiesDto,
                    Metadata = new PaginationMetadata
                    {
                        TotalCount = pagedActivities.Metadata.TotalCount,
                        PageSize = pagedActivities.Metadata.PageSize,
                        CurrentPage = pagedActivities.Metadata.CurrentPage,
                        TotalPages = pagedActivities.Metadata.TotalPages
                    }
                };

                return Result<PaginatedResult<ActivityDTO>>.SuccessResult(paginatedResult);
            }
        }
    }
}