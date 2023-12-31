﻿using Xunit;
using Moq;
using Application.Activities;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Interfaces;
using Domain.DTO;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests
{
    public class CreateHandlerTests
    {
        private readonly Mock<IActivityRepository> activityRepositoryMock;
        private readonly Mock<IAccessUser> accessUserMock;
        private readonly Mock<IMapper> mapperMock;

        public CreateHandlerTests()
        {
            activityRepositoryMock = new Mock<IActivityRepository>();
            accessUserMock = new Mock<IAccessUser>();
            mapperMock = new Mock<IMapper>();
        }

        private Activity CreateSampleActivity()
        {
            return new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                cancelled = false,
                Description = "This is a sample activity",
                Category = "Sports",
                City = "New York",
                Date = DateTime.UtcNow.AddDays(7),
                Venue = "Central Park",
                Messages = new List<Message>(),
                Attendees = new List<ActivityAttendee>()
            };
        }

        [Fact]
        public async Task Handle_GivenNullActivity_ShouldReturnFailure()
        {
            // Arrange
            var handler = new Create.Handler(activityRepositoryMock.Object, accessUserMock.Object, mapperMock.Object);
            var command = new Create.Command { Activity = null };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Activity instance cannot be null.", result.Error);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            // Arrange
            var handler = new Create.Handler(activityRepositoryMock.Object, accessUserMock.Object, mapperMock.Object);
            var newActivity = CreateSampleActivity();
            var command = new Create.Command { Activity = newActivity };

            activityRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Activity>()))
                .Returns(Task.CompletedTask)
                .Verifiable("Repository method AddAsync was not called with an Activity object.");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            activityRepositoryMock.Verify();
        }
    }
}