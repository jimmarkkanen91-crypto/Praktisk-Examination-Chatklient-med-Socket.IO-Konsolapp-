namespace ConsoleApp3
{
    public class MessagePayload
    {
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public string Username { get; set; }

        // Constructor to initialize the MessagePayload
        public MessagePayload(string username, string status = "Message")
        {
            Status = status;
            Timestamp = DateTimeOffset.UtcNow.ToString("o");
            Username = username;
        }
    }
}