using AutoMapper;
using Domain.DTO;
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
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<MessageDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var messages = await _context.Messages
                    .Include(m => m.User)
                    .ThenInclude(u => u.Image)
                    .Where(m => m.Activity.Id == request.Id)
                    .ToListAsync(cancellationToken);

                var messagesDto = _mapper.Map<List<MessageDTO>>(messages);

                return Result<List<MessageDTO>>.SuccessResult(messagesDto);
            }
        }
    }
}
