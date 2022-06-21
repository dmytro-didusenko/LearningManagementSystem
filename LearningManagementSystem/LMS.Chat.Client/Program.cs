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

        Console.WriteLine("Client is starting...");
        try
        {
            await hubConnection.InvokeAsync("Handshake", userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        await LoadChatHistoryAsync();

        bool isExit = false;
        while (!isExit)
        {
            await ReadAndSendMessage(isExit);
        }

        await hubConnection.StopAsync();
        Console.ReadLine();
    }

    public static async Task ReadAndSendMessage(bool isExit)
    {
        Console.Write("Enter message: ");
        var msg = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(msg))
        {
            return;
        }

        if (msg.Equals("/exit"))
        {
            isExit = true;
            return;
        }

        var chatMessage = new ChatMessage()
        {
            Text = msg,
            Date = DateTime.Now
        };
        await hubConnection.SendAsync("MessageHandler", chatMessage);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"\n\t\t\t\tMe-> {chatMessage.Text}" +
                          $"\n\t\t\t\t[{chatMessage.Date.ToShortDateString()}]");
        Console.ResetColor();
        await Task.Delay(20000);
    }

    public static async Task<bool> InitConnection()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/chat")
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<ChatMessage>("Send", async m =>
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n{m.Sender}-> {m.Text}" +
                              $"\n[{m.Date.ToShortDateString()}]");
            Console.ResetColor();
            await Task.Delay(20000);
        });

        hubConnection.On<ChatServerResponse>("Disconnect", (response) =>
       {
           Console.ForegroundColor = ConsoleColor.DarkRed;
           Console.WriteLine($"Message from server!");
           Console.WriteLine($"->{response.Message}");
           Console.WriteLine("Closing connection...");
           Console.ResetColor();
            //await hubConnection.StopAsync();
        });

        hubConnection.Closed += HubConnectionClosed;
        hubConnection.Reconnecting += HubConnectionReconnecting;

        await hubConnection.StartAsync();
        return hubConnection.State == HubConnectionState.Connected;
    }

    private static async Task HubConnectionClosed(Exception? arg)
    {
        Console.WriteLine("Connection closed. Retrying...");
        Console.WriteLine(arg.Message);
        TimeSpan retryDuration = TimeSpan.FromSeconds(10);
        DateTime retryTill = DateTime.UtcNow.Add(retryDuration);

        while (DateTime.UtcNow < retryTill)
        {
            bool connected = await InitConnection();
            if (connected)
                return;
        }

        Console.WriteLine("Connection closed");
    }

    private static Task HubConnectionReconnecting(Exception? arg)
    {
        Console.WriteLine("Reconnecting...");
        Console.WriteLine(arg?.Message);
        return Task.CompletedTask;
    }

    public static async Task LoadChatHistoryAsync()
    {
        var history = await hubConnection.InvokeAsync<ChatHistory>("GetChatHistory");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\t\t\t\tYou entered to group: {history.GroupName}");
        Console.ResetColor();

        if (history.ChatMessages is not null && history.ChatMessages.Any())
        {
            foreach (var message in history.ChatMessages)
            {

                if (message.Sender.Equals("Me"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"\n\t\t\t\t{message.Sender}-> {message.Text}" +
                                      $"\n\t\t\t\t[{message.Date.ToShortDateString()}]");
                    Console.ResetColor();
                    continue;
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\n{message.Sender}-> {message.Text}" +
                                  $"\n[{message.Date.ToShortDateString()}]");
                Console.ResetColor();
            }
        }
    }
}