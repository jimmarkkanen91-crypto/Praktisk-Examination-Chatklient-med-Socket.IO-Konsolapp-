namespace ConsoleApp3;

using System;
using System.Text.Json;

public static class MessageHandler
{
    // Sets up a handler for incoming messages on the socket
    public static void Setup(SocketIOClient.SocketIO socket)
    {
        socket.OnAny((eventName, response) =>
        {
            try
            {
                // Get the JSON from the server
                var root = response.GetValue<JsonElement>();

                // If the server sends an array, just take the first item
                var msg = root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0
                    ? root[0]
                    : root;

                // Only process JSON objects
                if (msg.ValueKind != JsonValueKind.Object) return;

                // Helper: safely get string properties
                string GetProp(string propName) =>
                    msg.TryGetProperty(propName, out var p)
                        ? (p.ValueKind == JsonValueKind.String ? p.GetString() ?? "" : p.ToString())
                        : "";

                // Only care about these statuses
                var status = GetProp("Status");
                if (status != "Message" && status != "Connected" && status != "Disconnected") return;

                // Who sent the message
                var user = string.IsNullOrEmpty(GetProp("Username")) ? "Unknown" : GetProp("Username");

                // Get timestamp or use current time if missing/invalid
                var time = DateTimeOffset.TryParse(GetProp("Timestamp"), out var dto)
                    ? dto.ToString("HH:mm:ss")
                    : DateTimeOffset.UtcNow.ToString("HH:mm:ss");

                // Show message text if normal message, otherwise show status
                var text = status == "Message" ? GetProp("Message") : status;

                // Print to console safely
                lock (Console.Out)
                {
                    Console.WriteLine($"\n{user} [{time}]: {text}");
                    Console.Write("Enter message to send (or press Enter to exit): ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading message: {e.Message}");
            }
        });
    }
}
