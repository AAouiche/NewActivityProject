using Domain.Interfaces;
using Domain.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context;
            private readonly IMessageRepository _messageRepository;

            public Handler(AppDbContext context,IMessageRepository messageRepository)
            {
                _messageRepository= messageRepository;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var message = await _messageRepository.FindById(request.Id);

                if (message == null)
                {
                    return Result<Unit>.Failure("Message not found");
                }

                
                var success = await _messageRepository.Delete(message);

                if (success)
                {
                    return Result<Unit>.SuccessResult(Unit.Value);
                }

                return Result<Unit>.Failure("Error deleting the message");
            }
        }
    }
}
