using Domain.Models;
using System;

namespace Domain.Interfaces
{
    public interface IAccessUser
    {
        public string GetUser();
        string GetUsername();
        Task<ApplicationUser> GetUserById(string userId);
    }
}