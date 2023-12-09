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
        private readonly IAccessUser  _accessUser;

        public ActivityRepository(AppDbContext context, IAccessUser accessUser)
        {
            _context = context;
            _accessUser = accessUser;
        }

        public async Task<Activity> GetByIdAsync(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            return activity;
        }

        public async Task<PaginatedResult<Activity>> GetAllAsync(int pageNumber, int pageSize, string filter = "all", DateTime? selectedDate = null)
        {
            
            var query = _context.Activities
                        .Include(a => a.Attendees)
                            .ThenInclude(att => att.ApplicationUser)
                                .ThenInclude(user => user.Image)
                        .AsQueryable();

           
            var currentUserId = _accessUser.GetUser();

           
            if (!string.IsNullOrEmpty(filter) && filter != "all")
            {
                if (filter == "going")
                {
                    query = query.Where(a => a.Attendees.Any(att => att.ApplicationUserId == currentUserId && !att.IsHost));
                }
                else if (filter == "hosting")
                {
                    query = query.Where(a => a.Attendees.Any(att => att.ApplicationUserId == currentUserId && att.IsHost));
                }
            }

            if (selectedDate.HasValue)
            {
                query = query.Where(a => a.Date.HasValue && a.Date.Value.Date == selectedDate.Value.Date);
            }

           
            var count = await query.CountAsync();

            
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            pageNumber = Math.Max(1, pageNumber);
            pageNumber = totalPages > 0 ? Math.Min(totalPages, pageNumber) : 1;



            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            
            return new PaginatedResult<Activity>
            {
                Items = items,
                Metadata = new PaginationMetadata
                {
                    TotalCount = count,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages
                }
            };
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