﻿using Application.Activities;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class EditHandlerTests
    {
        private readonly Mock<IActivityRepository> mockActivityRepository;
        private readonly Edit.Handler handler;

        public EditHandlerTests()
        {
            mockActivityRepository = new Mock<IActivityRepository>();
            handler = new Edit.Handler(mockActivityRepository.Object);
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
        public async Task Handle_ActivityFound_ShouldUpdateAndReturnSuccess()
        {
            // Arrange
            var sampleActivity = CreateSampleActivity();
            var updatedActivity = new Activity
            {
                Id = sampleActivity.Id,
                Title = "Updated Title",
                Description = "Updated Description"
               
            };

            mockActivityRepository.Setup(repo => repo.GetByIdAsync(sampleActivity.Id))
                .ReturnsAsync(sampleActivity);
            mockActivityRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Activity>()))
                .Returns(Task.CompletedTask);

           
            var command = new Edit.Command { Activity = updatedActivity };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Updated Title", sampleActivity.Title); 
            Assert.Equal("Updated Description", sampleActivity.Description);
        }

        [Fact]
        public async Task Handle_ActivityNotFound_ShouldReturnFailure()
        {
            // Arrange
            var nonExistentActivity = new Activity { Id = Guid.NewGuid() };
            mockActivityRepository.Setup(repo => repo.GetByIdAsync(nonExistentActivity.Id))
                .ReturnsAsync((Activity)null);

            //var handler = new Edit.Handler(mockActivityRepository.Object);
            var command = new Edit.Command { Activity = nonExistentActivity };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Could not find activity", result.Error);
        }

        [Fact]
        public async Task Handle_UpdateThrowsException_ShouldReturnFailure()
        {
            // Arrange
            var sampleActivity = CreateSampleActivity();
            mockActivityRepository.Setup(repo => repo.GetByIdAsync(sampleActivity.Id))
                .ReturnsAsync(sampleActivity);
            mockActivityRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Activity>()))
                .ThrowsAsync(new Exception("Database update error"));

            
            var command = new Edit.Command { Activity = sampleActivity };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.StartsWith("An error occurred while updating the activity:", result.Error);
        }

        
        [Fact]
        public async Task Handle_InvalidCommand_ShouldFailValidation()
        {
            // Arrange
            var invalidCommand = new Edit.Command { Activity = null }; 

            // Act
            var validationResults = new Edit.CommandValidator().Validate(invalidCommand);

            // Assert
            Assert.False(validationResults.IsValid);
        }
    }
}