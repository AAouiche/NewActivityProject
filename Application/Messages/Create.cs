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
        }
        public  class Handler : IRequestHandler<Command,Result<MessageDTO>>
            {
                private readonly AppDbContext _context;
                private readonly IAccessUser _accessUser;
                private readonly IActivityRepository _activityRepository;
                private readonly IMapper _mapper;
                private readonly IAccountRepository _accountRepository;
                private readonly IMessageRepository _messageRepository;
                public Handler(AppDbContext context,IAccessUser accessUser, IActivityRepository activityRepository, IMapper mapper,IAccountRepository accountRepository,IMessageRepository messageRepository)
                {
                _messageRepository = messageRepository;
                    _accountRepository = accountRepository;
                    _context = context;
                    _accessUser = accessUser;
                    _activityRepository = activityRepository;
                    _mapper = mapper;
                }
                public async Task<Result<MessageDTO>> Handle(Command request, CancellationToken cancellationToken)
                {

                var user = await _accountRepository.GetUserByIdWithImagesAsync();
                    var activity = await _activityRepository.GetByIdAsync(request.ActivityId);
                    var message = new Message
                    {
                        MessageBody = request.MessageBody,
                        User = user,
                        Activity = activity
                    };

                    
                    var success = await _messageRepository.AddMessageAsync(message);
                    if (success)
                    {
                        return Result<MessageDTO>.SuccessResult(_mapper.Map<MessageDTO>(message));
                    }
                    return Result<MessageDTO>.Failure("failed");
                   
                }

            }
            }
        
    }

