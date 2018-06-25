using Microsoft.AspNetCore.SignalR;
using SupportCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.App.Classes
{
    public class SignalrHub:Hub
    {
        public readonly static SignalRConnection<string> _connections = new SignalRConnection<string>();
        public async Task SendMessageAll(string user, string message)
        {
            string[] ExceptCon = _connections.GetConnections(user).Cast<string>().ToArray();
            await Clients.AllExcept(ExceptCon).SendAsync("ReceiveMessage",user,message);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }
        public override async Task OnConnectedAsync()
        {
            string UserId = Context.UserIdentifier??"Server";
            //Signalcon.Add(new SignalRcon { UserId=UserId, ConnectionId=Context.ConnectionId });
            _connections.Add(UserId, Context.ConnectionId);
            //await Groups.AddToGroupAsync(Context.ConnectionId,GroupName);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier);
            string UserId = Context.UserIdentifier ?? "Server";
            _connections.Remove(UserId, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

