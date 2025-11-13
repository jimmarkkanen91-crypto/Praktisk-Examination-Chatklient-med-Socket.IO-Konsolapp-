# ConsoleApp3 - C# Socket.IO Chat Client

A simple console-based chat client in C# using Socket.IO for real-time messaging. This project allows multiple users to connect to a Socket.IO server and exchange messages in real-time.

---

## Features

- Connect to a Socket.IO server
- Send and receive messages in real-time
- Displays message timestamp and sender username
- Handles server connection, disconnection, and graceful shutdown
- Supports multiple sessions by running multiple instances of the program

---

## Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) or later
- A running Socket.IO server
- Recommended IDE: JetBrains Rider, Visual Studio, or Visual Studio Code

---

## Controls

Type a message and press Enter — Sends the message to the server.

Press Enter on an empty line — Disconnects from the server and exits the program.

Press Ctrl+C — Gracefully shuts down the program and disconnects from the server.

Run multiple instances — Open multiple terminal windows and start separate instances to simulate multiple users.
