using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Security;
using Domain.Validation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class Create
    {
        public class Command: IRequest<Result<MessageDTO>>
        {
            public string MessageBody { get; set; }
            public Guid ActivityId { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.MessageBody).NotEmpty();
            }
            public  class Handler : IRequestHandler<Command,Result<MessageDTO>>
            {
                private readonly AppDbContext _context;
                private readonly IAccessUser _accessUser;
                private readonly IActivityRepository _activityRepository;
                private readonly IMapper _mapper;
                public Handler(AppDbContext context,IAccessUser accessUser, IActivityRepository activityRepository, IMapper mapper)
                {
                    _context = context;
                    _accessUser = accessUser;
                    _activityRepository = activityRepository;
                    _mapper = mapper;
                }
                public async Task<Result<MessageDTO>> Handle(Command request, CancellationToken cancellationToken)
                {

                    var user = await _context.Users
                        .Include(u => u.Image)
                        .FirstOrDefaultAsync(x => x.Id == _accessUser.GetUser());
                    var activity = await _activityRepository.GetByIdAsync(request.ActivityId);
                    var message = new Message
                    {
                        MessageBody = request.MessageBody,
                        User = user,
                        Activity = activity
                    };

                    await _context.Messages.AddAsync(message);
                    var success = await _context.SaveChangesAsync()>0;
                    if (success)
                    {
                        return Result<MessageDTO>.SuccessResult(_mapper.Map<MessageDTO>(message));
                    }
                    return Result<MessageDTO>.Failure("failed");
                   
                }

            }
            }
        }
    }

