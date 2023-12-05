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
            //using var context = CreateInMemoryDbContext();
            var handler = new Delete.Handler(activityRepositoryMock.Object);
            var command = new Delete.Command { Id = Guid.NewGuid() };

            
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
            
            var sampleActivity = CreateSampleActivity();
            
            
            activityRepositoryMock.Setup(repo => repo.GetByIdAsync(sampleActivity.Id))
                .ReturnsAsync(sampleActivity);
            activityRepositoryMock.Setup(repo => repo.DeleteAsync(sampleActivity.Id))
             .Returns(Task.CompletedTask);

            var handler = new Delete.Handler(activityRepositoryMock.Object);
            var command = new Delete.Command { Id = sampleActivity.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);

            // Verify that GetByIdAsync was called with the correct ID
            activityRepositoryMock.Verify(repo => repo.GetByIdAsync(sampleActivity.Id), Times.Once);

            // Verify that DeleteAsync was called with the correct ID
            activityRepositoryMock.Verify(repo => repo.DeleteAsync(sampleActivity.Id), Times.Once);
        }


    }
}
