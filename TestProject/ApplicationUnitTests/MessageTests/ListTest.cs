using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Application.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class ListTests
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly List.Handler _handler;

        public ListTests()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new List.Handler( _mockMapper.Object, _mockMessageRepository.Object);
        }

        private List<Message> CreateTestMessages(Guid activityId)
        {
            return new List<Message>
            {
                new Message
                {
                    Id = 1,
                    Activity = new Activity { Id = activityId },
                    MessageBody = "Test Message 1",
                    User = new ApplicationUser { UserName = "User1" },
                    
                },
                
            };
        }

        [Fact]
        public async Task Handle_ValidActivityId_ReturnsMessages()
        {
            // Arrange
            var activityId = Guid.NewGuid();
            var testMessages = CreateTestMessages(activityId);

            _mockMessageRepository.Setup(repo => repo.GetAllAsync(activityId))
                                  .ReturnsAsync(testMessages);
            _mockMapper.Setup(mapper => mapper.Map<List<MessageDTO>>(testMessages))
                       .Returns(new List<MessageDTO>()); // Mock the mapping result

            var query = new List.Query { Id = activityId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.IsType<List<MessageDTO>>(result.Value);
        }

        
    }
}