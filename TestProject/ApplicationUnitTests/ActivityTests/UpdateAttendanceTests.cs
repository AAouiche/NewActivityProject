using Application.Activities;
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
#pragma warning disable IDE1006 // Naming Styles
    public class updateAttendanceHandlerTests
#pragma warning restore IDE1006 // Naming Styles
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IAccessUser> _mockAccessUser;
        private readonly Mock<IActivityRepository> _mockActivityRepo;
        private readonly updateAttendance.Handler _handler;
        private readonly Mock<IAccountRepository> _mockAccountRepository;

        public updateAttendanceHandlerTests()
        {
            _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockAccessUser = new Mock<IAccessUser>();
            _mockActivityRepo = new Mock<IActivityRepository>();
            _mockAccountRepository= new Mock<IAccountRepository>();
            _handler = new updateAttendance.Handler(_mockContext.Object, _mockAccessUser.Object, _mockActivityRepo.Object,_mockAccountRepository.Object);
        }

        private Activity CreateTestActivity()
        {
            return new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                Description = "Description of the sample activity",
                Date = DateTime.UtcNow.AddDays(7),
                Category = "Category",
                City = "City",
                Venue = "Venue",


                Attendees = new List<ActivityAttendee>()
            };
        }

        private ApplicationUser CreateTestUser()
        {
            return new ApplicationUser
            {
                Id = "authenticated-user-id",
                UserName = "testuser"
                
            };
        }

        [Fact]
        public async Task Handle_ActivityDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockActivityRepo.Setup(repo => repo.GetActivityWithAttendees(It.IsAny<Guid>()))
                .ReturnsAsync((Activity)null);

            var command = new updateAttendance.Command { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var testActivity = CreateTestActivity();
            _mockActivityRepo.Setup(repo => repo.GetActivityWithAttendees(It.IsAny<Guid>()))
                .ReturnsAsync(testActivity);
            

            var command = new updateAttendance.Command { Id = testActivity.Id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_SuccessfulAttendanceUpdate_ReturnsSuccess()
        {
            // Arrange
            var testActivity = CreateTestActivity();
            var testUser = CreateTestUser();
            _mockActivityRepo.Setup(repo => repo.GetActivityWithAttendees(It.IsAny<Guid>()))
                .ReturnsAsync(testActivity);
            _mockAccessUser.Setup(user => user.GetUser())
                .Returns("authenticated-user-id");
            _mockAccountRepository.Setup(repo => repo.GetUserByIdAsync("authenticated-user-id"))
                .ReturnsAsync(testUser);
            _mockContext.Setup(ctx => ctx.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var command = new updateAttendance.Command { Id = testActivity.Id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }

       
    }
}