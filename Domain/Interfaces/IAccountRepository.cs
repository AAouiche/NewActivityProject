using Domain.DTO;
using Domain.Models;

using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task Register(RegisterDTO user);

    }
}
