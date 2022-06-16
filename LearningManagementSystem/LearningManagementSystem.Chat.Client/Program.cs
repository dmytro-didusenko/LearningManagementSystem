using Microsoft.AspNetCore.SignalR.Client;

namespace LearningManagementSystem.Chat.Client;

public class Program
{
    private static HubConnection hubConnection;
    public static async Task Main(string[] args)
    {
        await InitConnection();
        bool isExit = false;
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
                await hubConnection.SendAsync("MessageHandler", msg);
                Console.WriteLine("Message sent!");
            }
        }
    }

    public static async Task InitConnection()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/chat").Build();
        hubConnection.On<string>("Send", m =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"\n-> {m}");
            Console.ResetColor();
        });
        await hubConnection.StartAsync();
    }
}