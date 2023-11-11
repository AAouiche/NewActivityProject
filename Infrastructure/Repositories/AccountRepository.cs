using Domain.DTO;
using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository

       
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccessUser _accessUser;
        public AccountRepository(UserManager<ApplicationUser> userManager, IAccessUser accessuser)
        {
            _userManager = userManager;
            _accessUser = accessuser;
        }
        public async Task Register(RegisterDTO userDto)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = userDto.Email,
                Email = userDto.Email
               
            };

            
             await _userManager.CreateAsync(applicationUser, userDto.Password);

           
        }
        public async Task EditUser(EditUserDTO editedUser)
        {
            
        }
    }
}
