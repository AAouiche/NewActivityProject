using Domain.DTO;
using Domain.Models;
using Domain.Validation;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts
{
    public class Login
    {

        public class Command : IRequest<Result<UserDTO>>
        {
            public LoginDTO LoginDto { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<UserDTO>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly TokenService _tokenService;
            private readonly AppDbContext _context;

            public Handler(UserManager<ApplicationUser> userManager, TokenService tokenService, AppDbContext context)
            {
                _userManager = userManager;
                _tokenService = tokenService;
                _context = context;
            }

            public async Task<Result<UserDTO>> Handle(Login.Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                     .Include(u => u.Image)
                     .SingleOrDefaultAsync(u => u.Email == request.LoginDto.Email);

                if (user == null) return Result<UserDTO>.Failure("Unauthorized");

                var result = await _userManager.CheckPasswordAsync(user, request.LoginDto.Password);

                if (result)
                {
                    var userDto = new UserDTO
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.Token(user),
                        DisplayName = user.DisplayName,
                        ImageUrl = user.Image?.Url
                    };

                    return Result<UserDTO>.SuccessResult(userDto);
                }

                return Result<UserDTO>.Failure("Unauthorized");
            }
        }
    }
}
