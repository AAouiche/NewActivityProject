using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.IntegrationTests
{
    public class SeedData
    {


        public static async Task SeedTestData(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "bob", Email = "bob@test.com", DisplayName = "Bob" },
            new ApplicationUser { UserName = "jane", Email = "jane@test.com", DisplayName = "Jane" }

        };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if (!context.Activities.Any())
            {
                var testUser = await userManager.FindByEmailAsync("bob@test.com");
                var testActivityId = new Guid("12345678-9abc-def0-1234-56789abcdef0");
                var activities = new List<Activity>

                {
             new Activity
             {
                 Id = testActivityId,
            Title = "Test Activity 1",
            Date = DateTime.UtcNow.AddDays(10),
            Description = "Description for Test Activity 1",
            Category = "Sports",
            City = "New York",
            Venue = "Central Park",
            Attendees = new List<ActivityAttendee>
            {
                new ActivityAttendee { ApplicationUser = testUser, IsHost = true }
            },
            Messages= new List<Message> 
            {
              
            }
             },


            };

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }


        }
    }
}
