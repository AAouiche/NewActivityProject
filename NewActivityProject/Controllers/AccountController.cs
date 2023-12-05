using Application.Activities;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Application.Accounts;
using Domain.Validation;
using NewActivityProject.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace NewActivityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, TokenService tokenService,IMediator mediator, AppDbContext context,
                         ILogger<AccountController> logger)
             : base(logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _mediator = mediator;
            _context = context;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            return HandleResults(await _mediator.Send(new Login.Command { LoginDto = loginDto }));
        }

        [HttpGet("getUser")]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            return HandleResults(await _mediator.Send(new GetUser.Command()));
        }
        [HttpPost("register")]
        public async Task<ActionResult<Unit>> Register(RegisterDTO user)
        {
           
            return HandleResults(await _mediator.Send(new Register.Command { User = user}));
        }
        [HttpPost("edit")]
        public async Task<ActionResult< EditUserDTO>> Edit(EditUserDTO user)
        {
            return HandleResults(await _mediator.Send(new EditUser.Command { User = user }));
        }
        [HttpPost("validateToken")]
        public IActionResult ValidateToken(TokenDTO token)
        {
            try
            {
                _tokenService.ValidateToken(token);
                return Ok();
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
