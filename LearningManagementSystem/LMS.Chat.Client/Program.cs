using LearningManagementSystem.Domain.ChatModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace LearningManagementSystem.Chat.Client;

public class Program
{
    private static HubConnection hubConnection;
    public static async Task Main(string[] args)
    {
        Console.Write("Enter your user id: ");
        var userId = Console.ReadLine();

        await InitConnection();
        bool isExit = false;
        
        Console.WriteLine("Client is starting...");

        var messages = await hubConnection.InvokeAsync<IEnumerable<ChatMessage>>("Handshake", userId);
        if (hubConnection.State == HubConnectionState.Disconnected)
        {
            return;
        }

        if (messages is not null && messages.Any())
        {
            foreach (var message in messages)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{message.Sender}-> {message.Text}" +
                                  $"\n[{message.Date.ToShortDateString()}]");
                Console.ResetColor();
            }
        }

        while (!isExit)
        {
            Console.Write("Enter message: ");
            var msg = Console.ReadLine();
            if (msg.Equals("exit"))
            {
                isExit = true;
            }
            else
            {
                await hubConnection.SendAsync("MessageHandler", new ChatMessage()
                {
                    Text = msg
                });
                Console.WriteLine("Message sent!");
            }
        }

        await hubConnection.StopAsync();
    }

    public static async Task InitConnection()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/chat")
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<ChatMessage>("Send", m =>
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n{m.Sender}-> {m.Text}" +
                              $"\n[{m.Date.ToShortDateString()}]");
            Console.ResetColor();
        });

        hubConnection.On<ChatServerResponse>("Disconnect", async (response) =>
       {
           Console.ForegroundColor = ConsoleColor.DarkRed;
           Console.WriteLine($"Message from server!");
           Console.WriteLine($"->{response.Message}");
           Console.WriteLine("Closing connection...");
           Console.ResetColor();
           await hubConnection.StopAsync();
       });

        await hubConnection.StartAsync();
    }
}