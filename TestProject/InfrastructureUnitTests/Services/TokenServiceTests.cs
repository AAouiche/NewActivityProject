using Domain.DTO;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.InfrastructureUnitTests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["JwtConfig:Key"]).Returns("YourVeryLongSecretKeyHereThatIsAtLeast16Characters");

           
            _tokenService = new TokenService(_mockConfiguration.Object);
        }

        [Fact]
        public void Token_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com", Id = "1" };

            // Act
            var token = _tokenService.Token(user);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void Token_NullUser_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _tokenService.Token(null));
        }

        [Fact]
        public void ValidateToken_ValidToken_ReturnsClaimsPrincipal()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com", Id = "1" };
            var validToken = _tokenService.Token(user);
            var tokenDto = new TokenDTO { Token = validToken };

            // Act
            var claimsPrincipal = _tokenService.ValidateToken(tokenDto);

            // Assert
            Assert.NotNull(claimsPrincipal);
            
        }

        [Fact]
        public void ValidateToken_InvalidToken_ThrowsSecurityTokenException()
        {
            // Arrange
            var tokenDto = new TokenDTO { Token = "invalidToken" };

            // Act & Assert
            Assert.Throws<SecurityTokenException>(() => _tokenService.ValidateToken(tokenDto));
        }

        [Fact]
        public void ValidateToken_NullOrEmptyToken_ThrowsArgumentException()
        {
            // Arrange
            var tokenDto = new TokenDTO { Token = string.Empty };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _tokenService.ValidateToken(tokenDto));
        }

        
    }
}
