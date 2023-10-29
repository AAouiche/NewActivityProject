using Domain.DTO;
using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository

       
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task Register(RegisterDTO userDto)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = userDto.Email,
                Email = userDto.Email
                // Assign other properties as needed
            };

            // Here you create the user and pass the password from DTO to be hashed by UserManager
             await _userManager.CreateAsync(applicationUser, userDto.Password);

           
        }
    }
}
