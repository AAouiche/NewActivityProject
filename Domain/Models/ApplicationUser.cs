
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public string? DisplayName { get; set; }
        public String? Biography { get; set; }
        
        public Image? Image { get; set; }

        public ICollection<ActivityAttendee>? ActivitiesAttending { get; set; } = new List<ActivityAttendee>();
    }
}
