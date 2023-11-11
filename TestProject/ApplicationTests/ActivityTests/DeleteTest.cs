using Application.Activities;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class DeleteHandlerTests
    {
        private readonly Mock<IActivityRepository> activityRepositoryMock;
        public DeleteHandlerTests()
        {
            activityRepositoryMock = new Mock<IActivityRepository>();
            

        }
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private Activity CreateSampleActivity()
        {
            return new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                Description = "Description of the sample activity",
                Date = DateTime.UtcNow.AddDays(7),
                Category = "Category",
                City = "City",
                Venue = "Venue"
                
            };
        }

        [Fact]
        public async Task Handle_ActivityNotFound_ShouldReturnFailure()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var handler = new Delete.Handler(activityRepositoryMock.Object);
            var command = new Delete.Command { Id = Guid.NewGuid() };

            // Mock repository setup
            activityRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Activity)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Activity not found", result.Error);
        }

        [Fact]
        public async Task Handle_ValidActivity_ShouldDeleteAndReturnSuccess()
        {
            // Arrange
            using var context = CreateInMemoryDbContext();
            var sampleActivity = CreateSampleActivity();
            context.Activities.Add(sampleActivity);
            context.SaveChanges();

            // Mock repository setup
            activityRepositoryMock.Setup(repo => repo.GetByIdAsync(sampleActivity.Id))
                .ReturnsAsync(sampleActivity);
            activityRepositoryMock.Setup(repo => repo.DeleteAsync(sampleActivity.Id))
             .Callback(async () =>
             {
            context.Activities.Remove(sampleActivity);
            await context.SaveChangesAsync(); 
             });

            var handler = new Delete.Handler(activityRepositoryMock.Object);
            var command = new Delete.Command { Id = sampleActivity.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            var deletedActivity = await context.Activities.FindAsync(sampleActivity.Id);
            Assert.Null(deletedActivity);
        }


    }
}
