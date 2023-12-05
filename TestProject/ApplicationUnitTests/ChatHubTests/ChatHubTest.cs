using Application.Messages;
using Domain.DTO;
using Domain.Validation;
using Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests
{
    public class ChatHubTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<IHubCallerClients> _mockClients;
        private readonly Mock<IClientProxy> _mockClientProxy;
        private readonly Mock<HubCallerContext> _mockContext;
        private readonly ChatHub _hub;

        public ChatHubTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockClients = new Mock<IHubCallerClients>();
            _mockClientProxy = new Mock<IClientProxy>();
            _mockContext = new Mock<HubCallerContext>();
            _hub = new ChatHub(_mockMediator.Object)
            {
                Clients = _mockClients.Object,
                Context = _mockContext.Object
            };
        }

        [Fact]
        public async Task SendMessage_CallsGroupWithMessage()
        {
            // Arrange
            var activityId = Guid.NewGuid();
            var command = new Create.Command
            {
                ActivityId = activityId,
                MessageBody = "Test message body"
            };

            var messageDto = new MessageDTO
            {
                Id = 1,
                MessageBody = "Test message",
                Created = DateTime.UtcNow,
                Username = "testuser",
                DisplayName = "Test User",
                Image = "testimage.jpg"
            };

            
            _mockMediator.Setup(m => m.Send(It.IsAny<Create.Command>(), default))
                         .ReturnsAsync(Result<MessageDTO>.SuccessResult(messageDto));
            _mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);

            // Act
            await _hub.SendMessage(command);

            // Assert
            _mockMediator.Verify(m => m.Send(It.Is<Create.Command>(c => c.ActivityId == activityId && c.MessageBody == "Test message body"), default), Times.Once);
            _mockClients.Verify(c => c.Group(activityId.ToString()), Times.Once);
            _mockClientProxy.Verify(p => p.SendCoreAsync("ReceiveMessage", It.Is<object[]>(o => o[0] == messageDto), default), Times.Once);
        }

        


    }
}