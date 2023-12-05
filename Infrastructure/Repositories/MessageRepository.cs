using Azure.Core;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Boolean> AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Message> FindById(int id)
        {
            return await _context.Messages.FindAsync(id);
        }
        public async Task<Boolean> Delete(Message message)
        {
            _context.Messages.Remove(message);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Message>> GetAllAsync(Guid id)
        {
            return await _context.Messages
                    .Include(m => m.User)
            .ThenInclude(u => u.Image)
                    .Where(m => m.Activity.Id == id)
                    .ToListAsync();
        }

    }  
}
