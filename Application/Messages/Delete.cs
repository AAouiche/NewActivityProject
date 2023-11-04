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

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var message = await _context.Messages.FindAsync(request.Id);

                if (message == null)
                {
                    return Result<Unit>.Failure("Message not found");
                }

                _context.Messages.Remove(message);
                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return Result<Unit>.SuccessResult(Unit.Value);
                }

                return Result<Unit>.Failure("Error deleting the message");
            }
        }
    }
}
