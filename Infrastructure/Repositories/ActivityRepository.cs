using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> GetByIdAsync(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<List<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(att => att.ApplicationUser)
                .ThenInclude(user => user.Image)
                .ToListAsync();
        }

        public async Task<List<Activity>> GetByUserIdAsync(string userId)
        {
            return await _context.Activities
                .Where(a => a.Attendees.Any(att => att.ApplicationUserId == userId))
                .Include(a => a.Attendees)
                .ThenInclude(a => a.ApplicationUser)
                .ToListAsync();
        }

        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var activity = await GetByIdAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Activity> GetByIdWithAttendeesAsync(Guid id)
        {

            return await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(att => att.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Activity>> GetActivitiesByUserIdAsync(string userId)
        {
            return await _context.Activities
                .Where(a => a.Attendees.Any(att => att.ApplicationUserId == userId))
                .Include(a => a.Attendees)
                .ThenInclude(att => att.ApplicationUser)
                .ToListAsync();
        }
        public async Task<Activity> GetActivityWithAttendees(Guid id)
        {
            return await _context.Activities
                .Include(a => a.Attendees).ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}