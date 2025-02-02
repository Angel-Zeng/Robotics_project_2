using SimpleMqtt;

Console.WriteLine("Starting MQTT Client...");

var client = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("client-Id");

// Attach event handler before subscribing
client.OnMessageReceived += (sender, args) =>
{
    Console.WriteLine($"[Received] Topic: {args.Topic}, Message: {args.Message}");
};

// Subscribe to the topic
await client.SubscribeToTopic("tasks");

Console.WriteLine("Subscribed to 'tasks'. Enter messages or type 'stop' to quit.");

while (true)
{
    Console.Write("Message: ");
    string? message = Console.ReadLine();

    if (string.Equals(message, "stop", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!string.IsNullOrWhiteSpace(message))
    {
        await client.PublishMessage(message, "tasks");
        Console.WriteLine("Message sent.");
    }

    // Let the program continue listening for messages
}

client.Dispose();
Console.WriteLine("Client disposed. Exiting...");


