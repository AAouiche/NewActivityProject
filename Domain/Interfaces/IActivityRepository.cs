using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity> GetByIdAsync(Guid id);
        Task<PaginatedResult<Activity>> GetAllAsync(int pageNumber, int pageSize);
        Task<List<Activity>> GetByUserIdAsync(string userId);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(Guid id);
        Task<Activity> GetByIdWithAttendeesAsync(Guid id);
        Task<List<Activity>> GetActivitiesByUserIdAsync(string userId);
        Task<Activity> GetActivityWithAttendees(Guid id);
        
    }
}