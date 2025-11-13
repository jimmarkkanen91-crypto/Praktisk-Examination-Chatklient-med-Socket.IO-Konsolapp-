namespace ConsoleApp3;
using SocketIOClient;

public static class SocketManger
{
    // Method to set up and configure the SocketIO client
    public static SocketIOClient.SocketIO SetupSocket()
    {
        var socket = new SocketIOClient.SocketIO("https://api.leetcode.se", new SocketIOOptions
        {
            Path = "/sys25d",
            Reconnection = true,
            ReconnectionAttempts = 3,
            ReconnectionDelay = 1000
        });

        
        return socket;
    }
}