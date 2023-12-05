using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class List
    {
        public class Query : IRequest<Result<List<MessageDTO>>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<MessageDTO>>>
        {
            
            private readonly IMapper _mapper;
            private readonly IMessageRepository _messageRepository;

            public Handler( IMapper mapper, IMessageRepository messageRepository)
            {
                _messageRepository= messageRepository;
               
                _mapper = mapper;
            }

            public async Task<Result<List<MessageDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var messages = await _messageRepository.GetAllAsync(request.Id);

                var messagesDto = _mapper.Map<List<MessageDTO>>(messages);

                return Result<List<MessageDTO>>.SuccessResult(messagesDto);
            }
        }
    }
}
