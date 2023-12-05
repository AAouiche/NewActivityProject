using Domain.DTO;
using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository

       
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccessUser _accessUser;
        private readonly AppDbContext _context;
        public AccountRepository(UserManager<ApplicationUser> userManager, IAccessUser accessuser, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _accessUser = accessuser;
            _context = appDbContext;
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
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<ApplicationUser> GetUserByIdWithImagesAsync()
        {
            return await _context.Users
                        .Include(u => u.Image)
                        .FirstOrDefaultAsync(x => x.Id == _accessUser.GetUser());
        }
        
    }
}
