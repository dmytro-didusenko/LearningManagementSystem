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
        var ms = await hubConnection.InvokeAsync<ChatServerResponse>("Handshake", userId);
        if (!ms.IsSuccessful)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Server response: {ms.Message}");
            Console.WriteLine("Closing connection...");
            Console.ResetColor();
            Console.WriteLine(hubConnection.State);
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
        await hubConnection.StartAsync();
    }
}