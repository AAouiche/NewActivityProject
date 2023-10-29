using Domain.Models;

public class ActivityAttendee
{
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }

    public bool IsHost { get; set; } 

  
}