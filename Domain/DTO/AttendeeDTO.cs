using Domain.Models;

namespace Domain.DTO
{
    public class AttendeeDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public bool IsHost { get; set; }
        public string? ImageUrl { get; set; } 
    }
}
