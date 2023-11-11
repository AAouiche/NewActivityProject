using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Domain.Validation;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Application.Accounts
{
    public class EditUser
    {
        public class Command : IRequest<Result<EditUserDTO>>
        {
            public EditUserDTO User { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<EditUserDTO>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IAccessUser _accessUser;

            public Handler(UserManager<ApplicationUser> userManager, IAccessUser accessUser)
            {
                _userManager = userManager;
                _accessUser = accessUser;
            }

            public async Task<Result<EditUserDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userId = _accessUser.GetUser();
                var user = await _userManager.FindByIdAsync(userId);

                
                user.Email = request.User.Email;
                user.DisplayName = request.User.DisplayName;
                user.Biography = request.User.Biography;

               
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Result<EditUserDTO>.Failure("Profile update failed.");
                }

                
                var userDto = new EditUserDTO
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Biography = user.Biography
                };

                return Result<EditUserDTO>.SuccessResult(userDto);
            }
        }
    }
}
