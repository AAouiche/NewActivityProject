using Application.Messages;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class CreateTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IAccessUser> _mockAccessUser;
        private readonly Mock<IActivityRepository> _mockActivityRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Create.Handler _handler;
        private readonly Mock<IMessageRepository> _mockMessageRepository;

        public CreateTests()
        {
            _mockMessageRepository= new Mock<IMessageRepository>();
            _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockAccessUser = new Mock<IAccessUser>();
            _mockActivityRepo = new Mock<IActivityRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _handler = new Create.Handler(_mockContext.Object, _mockAccessUser.Object, _mockActivityRepo.Object, _mockMapper.Object, _mockAccountRepository.Object,_mockMessageRepository.Object);
        }

        private ApplicationUser CreateTestUser()
        {
            return new ApplicationUser
            {
                Id = "user123",
                UserName = "testuser",
                DisplayName = "Test User",
                Image = new Image
                {
                    Id = 1,
                    Url = "http://example.com/image.jpg",
                    FileName = "image.jpg",
                    Size = 1024,
                    ContentType = "image/jpeg",
                    ApplicationUserId = "user123"
                }
                
            };
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
                Venue = "Venue"
                
            };
        }

        private MessageDTO CreateTestMessageDTO(Message message)
        {
            return new MessageDTO
            {
                Id = new Random().Next(1, 1000),
                MessageBody = message.MessageBody,
                Created = DateTime.UtcNow,
                Username = message.User.UserName,
                Image = message.User.Image?.Url,
                DisplayName = message.User.DisplayName
            };
        }

        [Fact]
        public async Task Handle_ValidMessage_CreatesMessageSuccessfully()
        {
            // Arrange
            var testUser = CreateTestUser();
            var testActivity = CreateTestActivity();

            _mockAccessUser.Setup(x => x.GetUser()).Returns(testUser.Id);
            _mockAccountRepository.Setup(repo => repo.GetUserByIdWithImagesAsync())
                                  .ReturnsAsync(testUser);
            _mockActivityRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(testActivity);
            _mockMapper.Setup(mapper => mapper.Map<MessageDTO>(It.IsAny<Message>()))
                       .Returns((Message m) => CreateTestMessageDTO(m));
            _mockMessageRepository.Setup(x => x.AddMessageAsync(It.IsAny<Message>()))
                .ReturnsAsync(true);

            var command = new Create.Command
            {
                MessageBody = "Test message",
                ActivityId = testActivity.Id
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.Equal("Test message", result.Value.MessageBody);
            Assert.Equal(testUser.UserName, result.Value.Username);
        }

        
    }
}