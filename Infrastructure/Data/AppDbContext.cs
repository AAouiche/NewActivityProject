using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Reflection.Metadata;


public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> Attendees { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Message> Messages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityAttendee>()
        .HasKey(aa => new { aa.ApplicationUserId, aa.ActivityId });

        modelBuilder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.ApplicationUser)
            .WithMany(u => u.ActivitiesAttending)
            .HasForeignKey(aa => aa.ApplicationUserId);

        modelBuilder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(aa => aa.ActivityId);
        modelBuilder.Entity<Message>()
            .HasOne(a=> a.Activity)
            .WithMany(u => u.Messages)
            .OnDelete(DeleteBehavior.Cascade);

        /* modelBuilder.Entity<Activity>().HasData(
             new Activity
             {
                 Id = Guid.NewGuid(),
                 Title = "Activity 1",
                 Description = "Description 1",
                 Category = "Category 1",
                 City = "City 1",
                 Date = DateTime.Now,
                 Venue = "Venue 1"
             },
             new Activity
             {
                 Id = Guid.NewGuid(),
                 Title = "Activity 2",
                 Description = "Description 2",
                 Category = "Category 2",
                 City = "City 2",
                 Date = DateTime.Now,
                 Venue = "Venue 2"
             }

         );*/
    }
}