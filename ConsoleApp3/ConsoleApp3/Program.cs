namespace ConsoleApp3;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Socket instance
    private static SocketIOClient.SocketIO _socket = null!;

    static async Task Main(string[] args)
    {
        Console.CursorTop++;
        const string EventName = "message"; // Event name to use
        var cts = new CancellationTokenSource();

        // Require a non-empty username
        string username;
        do
        {
            Console.Write("Enter your name: ");
            username = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Name cannot be empty. Please enter a name.");
            }
        } while (string.IsNullOrWhiteSpace(username));

        // Initialize the socket
        _socket = SocketManger.SetupSocket(); 
        
        // Setup message handler
        MessageHandler.Setup(_socket); 

        // Show when the socket connects
        _socket.OnConnected += async (s, e) =>
        {
            Console.WriteLine("Connected to server.");
            await ConnectDisconnect.ConnectPayLoad(_socket, EventName, username);
            Ask();
        };
        // Start connection
        Console.WriteLine("Connecting...");
        try
        {
            await _socket.ConnectAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connect failed: {ex}");
            return;
        }
        // Task to handle sending messages
        var sendTask = Task.Run(async () =>
        {
            while (!cts.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Disconnecting from server...");
                    await ConnectDisconnect.DisconnectPayLoad(_socket, EventName, username);
                    cts.Cancel();
                    break;
                }

                // Create message payload
                var messagePayload = new[]
                {
                    new
                    {
                        Status = "Message",
                        Timestamp = DateTimeOffset.UtcNow.ToString("o"),
                        Username = username,
                        Message = input
                    }
                };
                // Send the message
                try
                {
                    await _socket.EmitAsync(EventName, messagePayload);
                    Console.WriteLine("Message sent");
                    Ask();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Send failed: {ex.Message}");
                }
            }
        }, cts.Token);

        await sendTask;
        
        // Clean up on exit
        try
        {
            if (_socket.Connected)
            {
                await _socket.DisconnectAsync();
            }
        }
        catch
        {
            // ignore errors during disconnect
        }
        
        _socket.Dispose();
        Console.WriteLine("Socket disposed. Exiting application.");
        
        
        // Show when the socket disconnects (network/server)
        _socket.OnDisconnected += (s, e) =>
        {
            Console.WriteLine("Disconnected from server.");
        };
        
        // Handle Ctrl+C to gracefully disconnect
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true; // prevent immediate termination
            Console.WriteLine("Shutting down...");
            Task.Run(async () =>
            {
                try
                {
                    await ConnectDisconnect.DisconnectPayLoad(_socket, EventName, username);
                    await _socket.DisconnectAsync();
                }
                catch
                {
                    // ignore errors on shutdown
                }
                cts.Cancel();
            });
        };

    
            
    }
    
    // Prompt user for input
    private static void Ask() => Console.Write("Enter message to send (or press Enter to exit): ");

    
}