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
    public class RegisterHandlerTests
    {
        private readonly Mock<IAccessUser> _mockAccessUser;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Register.Handler _handler;

        public RegisterHandlerTests()
        {
            _mockAccessUser = new Mock<IAccessUser>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockUserManager = MockHelpers.MockUserManager<ApplicationUser>(); // Use MockHelpers here
            _handler = new Register.Handler(_mockAccountRepository.Object, _mockAccessUser.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_SuccessfulRegistration_ReturnsSuccess()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Email = "test@example.com",
                Password = "Password123!"
                
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            var command = new Register.Command { User = registerDto };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(registerDto, result.Value);
        }

        [Fact]
        public async Task Handle_UserIsNull_ReturnsFailure()
        {
            // Arrange
            var command = new Register.Command { User = null };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User instance cannot be null.", result.Error);
        }

        [Fact]
        public async Task Handle_EmailAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var registerDto = new RegisterDTO { Email = "existing@example.com", Password = "Password123!" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email))
                            .ReturnsAsync(new ApplicationUser { Email = registerDto.Email });

            var command = new Register.Command { User = registerDto };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("A user with this email already exists.", result.Error);
        }

       
    }
}
