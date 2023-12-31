﻿namespace Domain.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
#pragma warning disable IDE1006 // Naming Styles
        public bool? cancelled { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? City { get; set; }
        public DateTime? Date { get; set; }
        public string? Venue { get; set; }
        
        public ICollection<Message>? Messages { get; set; } = new List<Message>();
        public ICollection<ActivityAttendee>? Attendees { get; set; } = new List<ActivityAttendee>();
    }
}
