using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Core.Hubs
{
    public class StaffChatHub : Hub
    {
        private readonly AppDbContext _context;

        public StaffChatHub(AppDbContext context)
        {
            _context = context;
        }
        public async Task Handshake(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedId))
                Context.Abort();

            var user = await _context.Users.FindAsync(parsedId);
            var nullIfStaff = await _context.Students.FindAsync(parsedId);

            if (user is null || nullIfStaff != null)
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");
            Context.Items.TryAdd("User", user);
        }
        public async Task<IEnumerable<ChatMessage>> GetChatHistory()
        {
            IEnumerable<ChatMessage> messages = await _context.StaffChatMessages
                .Select(m => new ChatMessage()
                {
                    Sender = m.Sender.UserName,
                    Text = m.Text,
                    Date = m.CreationDate
                })
                .ToListAsync();
            
            return messages;
        }
        public async Task MessageHandler(ChatMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
                return;
            
            var sender = Context.Items["User"] as User;
            message.Date = DateTime.Now;

            await _context.StaffChatMessages.AddAsync(new StaffChatMessage()
            {
                SenderId = sender.Id,
                CreationDate = message.Date,
                Text = message.Text,
            });
            await _context.SaveChangesAsync();

            await Clients.Group("Staff").SendAsync("ReceiveMessage", message);
        }
    }
}
