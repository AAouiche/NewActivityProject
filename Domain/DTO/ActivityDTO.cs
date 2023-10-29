namespace Domain.DTO
{
    public class ActivityDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? City { get; set; }
        public DateTime? Date { get; set; }
        public string? Venue { get; set; }
        public bool? cancelled { get; set; }
        public AttendeeDTO Host { get; set; }
        public ICollection<AttendeeDTO> Attendees { get;set; }
    }
}
