namespace ConsoleApp3;


public static class ConnectDisconnect
{
    // Method to send a connect payload
    public static async Task ConnectPayLoad(SocketIOClient.SocketIO socket, string eventName, string username)
    {
        try
        {
            var payload = new[]
            {
                new MessagePayload(username, status: "Connected")
            };

            await socket.EmitAsync(eventName, payload);
            //Console.WriteLine($"Sent {eventName} with payload: {System.Text.Json.JsonSerializer.Serialize(payload)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Join failed: {ex.Message}");
        }
    }

    // Method to send a disconnect payload
    public static async Task DisconnectPayLoad(SocketIOClient.SocketIO socket, string eventName,
        string username)
    {
        try
        {
            var payload = new[]
            {
                new MessagePayload(username, status: "Disconnected")
            };

            await socket.EmitAsync(eventName, payload);
            // Console.WriteLine($"Sent {eventName} with payload: {System.Text.Json.JsonSerializer.Serialize(payload)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Disconnect failed: {ex.Message}");
        }
    }
    
}