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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, TokenService tokenService,IMediator mediator, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _mediator = mediator;
            _context = context;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO LoginDto)
        {
            var user = await _context.Users
             .Include(u => u.Image)
             .SingleOrDefaultAsync(u => u.Email == LoginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, LoginDto.Password);

            if (result)
            {
                return new UserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.Token(user),
                    DisplayName= user.DisplayName,
                    ImageUrl = user.Image?.Url
                   
                    

                };
            }

            return Unauthorized();
        }

        [HttpGet("getUser")]
          
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var user = await _context.Users
            .Include(u => u.Image)
            .SingleOrDefaultAsync(u => u.Email == User.FindFirstValue(ClaimTypes.Email));

            if (user == null) return NotFound("User not found");

            var userDto = new UserDTO
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                ImageUrl = user.Image?.Url
                  
            };

            return Ok(userDto);
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
