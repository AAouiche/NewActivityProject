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
        public class Query : IRequest<Result<List<ActivityDTO>>> { }  // Change return type to ActivityDTO

        public class Handler : IRequestHandler<Query, Result<List<ActivityDTO>>> // Change return type to ActivityDTO
        {
            private readonly IActivityRepository _activityRepository;
            private readonly IAccessUser _accessUser;
            private readonly IMapper _mapper;  
            private readonly IBlobStorageService _blobStorageService;

            public Handler(IActivityRepository activityRepository, IAccessUser accessUser, IMapper mapper, IBlobStorageService blobStorageService)  // Add IMapper to constructor
            {
                _activityRepository = activityRepository;
                _accessUser = accessUser;
                _mapper = mapper;
                _blobStorageService = blobStorageService;
            }

            public async Task<Result<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var currentUserId = _accessUser.GetUser();

                var activities = await _activityRepository.GetAllAsync();

                
                var activitiesDto = _mapper.Map<List<ActivityDTO>>(activities);

                return Result<List<ActivityDTO>>.SuccessResult(activitiesDto);
            }
        }
    }
}