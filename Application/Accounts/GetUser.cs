using Domain.DTO;
using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts
{
    public class GetUser
    {
        public class Command : IRequest<Result<UserDTO>>
        {
        }
        public class Handler : IRequestHandler<GetUser.Command, Result<UserDTO>>
        {
            private readonly AppDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Result<UserDTO>> Handle(GetUser.Command request, CancellationToken cancellationToken)
            {
                var email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    return Result<UserDTO>.Failure("User not found");
                }

                var user = await _context.Users
                    .Include(u => u.Image)
                    .SingleOrDefaultAsync(u => u.Email == email);

                if (user == null) return Result<UserDTO>.Failure("User not found");

                var userDto = new UserDTO
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.Image?.Url
                };

                return Result<UserDTO>.SuccessResult(userDto);
            }
        }
    }
}
