using Application.Messages;
using Domain.Interfaces;
using Infrastructure.Migrations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub( IMediator mediator) 
        {
            _mediator = mediator;
        }
        public async Task SendMessage(Create.Command command)
        {
            var message = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveMessage", message.Value);
        }
        public override async Task OnConnectedAsync()
        {
            var activityId = Context.GetHttpContext().Request.Query["activityId"].ToString();

            if (!string.IsNullOrEmpty(activityId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

                // Fetch the list of messages for the given activityId
                var messages = await _mediator.Send(new List.Query { Id = Guid.Parse(activityId) });

               
                await Clients.Caller.SendAsync("ReceiveInitialMessages", messages.Value);
                
            }

            await base.OnConnectedAsync();
        }
    }
}
