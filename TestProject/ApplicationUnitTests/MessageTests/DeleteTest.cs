using Application.Messages;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Application.Tests
{
    public class DeleteTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Delete.Handler _handler;
        private readonly Mock<IMessageRepository> _mockMessageRepository;

        public DeleteTests()
        {
            _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockMessageRepository = new Mock<IMessageRepository>();
            _handler = new Delete.Handler(_mockContext.Object,_mockMessageRepository.Object);
           
        }

        private Message CreateTestMessage(int id)
        {
            return new Message
            {
                Id = id,
                MessageBody = "Sample message body",
                Activity = new Activity
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Activity",
                    
                },
                Created = DateTime.UtcNow,
                User = new ApplicationUser
                {
                    Id = "user123",
                    UserName = "testuser",
                   
                }
            };
        }

        [Fact]
        public async Task Handle_MessageExists_DeletesMessageSuccessfully()
        {
            // Arrange
            var testMessage = CreateTestMessage(1);
            
            _mockMessageRepository.Setup(m => m.FindById(testMessage.Id))
                .ReturnsAsync(testMessage);
            _mockMessageRepository.Setup(m => m.Delete(testMessage))
                .ReturnsAsync(true);

            var command = new Delete.Command { Id = 1 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_MessageNotFound_ReturnsFailure()
        {
            // Arrange
            _mockMessageRepository.Setup(m => m.FindById(1))
                .ReturnsAsync((Message)null); 

            var command = new Delete.Command { Id = 1 };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Message not found", result.Error);
        }


    }
}
