using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Interfaces;

namespace   Infrastructure.Security
{
    public class AccessUser : IAccessUser
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly AppDbContext _context;
        public AccessUser(IHttpContextAccessor HttpContextAccessor, AppDbContext context)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _context = context;
        }
        public string GetUser()
        {
            
            var userIdString = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                // Handle this case appropriately; here I'm throwing an exception, but you might handle it differently.
                throw new InvalidOperationException("User is not authenticated");
            }

            return userIdString;
        }
        public string GetUsername()
        {
            var username = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (username != null)
                return username;


            throw new InvalidOperationException("Username not found in claims");
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
