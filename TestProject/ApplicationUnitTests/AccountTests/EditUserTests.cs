using Application.Accounts;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Mock; 

namespace Application.Tests
{
    public class EditUserHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IAccessUser> _mockAccessUser;
        private readonly EditUser.Handler _handler;

        public EditUserHandlerTests()
        {
            _mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
            _mockAccessUser = new Mock<IAccessUser>();
            _handler = new EditUser.Handler(_mockUserManager.Object, _mockAccessUser.Object);
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsSuccess()
        {
            // Arrange
            var testUserId = "user123";
            var testUser = new ApplicationUser { Id = testUserId, Email = "original@example.com", DisplayName = "Original Name" };
            var editUserDto = new EditUserDTO { Email = "new@example.com", DisplayName = "New Name", Biography = "New Bio" };

            _mockAccessUser.Setup(x => x.GetUser()).Returns(testUserId);
            _mockUserManager.Setup(x => x.FindByIdAsync(testUserId)).ReturnsAsync(testUser);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var command = new EditUser.Command { User = editUserDto };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(editUserDto.Email, result.Value.Email);
            Assert.Equal(editUserDto.DisplayName, result.Value.DisplayName);
            Assert.Equal(editUserDto.Biography, result.Value.Biography);
        }

        [Fact]
        public async Task Handle_FailedUpdate_ReturnsFailure()
        {
            // Arrange
            var testUserId = "user123";
            var testUser = new ApplicationUser { Id = testUserId };
            var editUserDto = new EditUserDTO { /* Initialize properties */ };

            _mockAccessUser.Setup(x => x.GetUser()).Returns(testUserId);
            _mockUserManager.Setup(x => x.FindByIdAsync(testUserId)).ReturnsAsync(testUser);
            _mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Failed());

            var command = new EditUser.Command { User = editUserDto };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Profile update failed.", result.Error);
        }

       
    }
}
