using Domain.DTO;
using Domain.Models;


using Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Accounts
{
    public class Register
    {
        public class Command : IRequest<Result<RegisterDTO>>
        {
            public RegisterDTO User { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<RegisterDTO>>
        {
            private readonly IAccessUser _accessUser;
            private readonly IAccountRepository _accountRepository;
             private readonly UserManager<ApplicationUser> _userManager;

            public Handler(IAccountRepository accountRepository, IAccessUser accessUser, UserManager<ApplicationUser> userManager)
            {
                _accessUser = accessUser;
                _accountRepository = accountRepository;
                _userManager = userManager;
            }

            public async Task<Result<RegisterDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.User == null)
                {
                    return Result<RegisterDTO>.Failure("User instance cannot be null.");
                }

                
                var existingUser = await _userManager.FindByEmailAsync(request.User.Email);
                if (existingUser != null)
                {
                    return Result<RegisterDTO>.Failure("A user with this email already exists.");
                }

                
                var newUser = new ApplicationUser
                {
                    UserName = request.User.Email, 
                    Email = request.User.Email,
                    
                };

                var result = await _userManager.CreateAsync(newUser, request.User.Password);

                if (!result.Succeeded)
                {
                    return Result<RegisterDTO>.Failure("User could not be created.");
                   
                }

                

                return Result<RegisterDTO>.SuccessResult(request.User);
            }
        }
    }
}