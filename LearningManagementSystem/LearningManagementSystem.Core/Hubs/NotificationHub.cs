using LearningManagementSystem.API.SignalRServices;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using LearningManagementSystem.Domain.Models.NotificationMessage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;


namespace LearningManagementSystem.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly AppDbContext _db;
        private readonly IUserConnectionService userConnections;

        public NotificationHub(AppDbContext db, IUserConnectionService userConnections)
        {
            _db = db;
            this.userConnections = userConnections;
        }




        //public async Task SendMessage(Guid userId, NotificationMessage message)
        //{
        //    var userConnectionIds = connectionUsers[userId];
        //    if (userConnectionIds is not null)
        //    {
        //        foreach (var ConnectionId in userConnectionIds)
        //        {
        //            await Clients.User(ConnectionId).SendAsync("ShowNotification", message);
        //        }
        //    }
        //}


        public async Task AddUser(string userStringId)
        {
            Console.WriteLine(userStringId);

            if (!Guid.TryParse(userStringId, out var userId))
            {
                return;
            }

            var userFromDB = await _db.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if (userFromDB is not null)
            {
                userConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            else
            {
                Context.Abort();
            }

        }



        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId + "  is Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            userConnections.RemoveUserConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

