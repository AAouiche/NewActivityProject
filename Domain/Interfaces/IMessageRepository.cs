using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<Boolean> AddMessageAsync(Message message);
        Task<Message> FindById(int id);
        Task<Boolean> Delete(Message message);
        Task<List<Message>> GetAllAsync(Guid id);

    }
}
