// using SimpleMqtt;

// Console.WriteLine("Hello, World!");

// var client = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("client-Id");

// await client.PublishMessage("Hello World", "Hehe");

// client.OnMessageReceived += (sender, args) =>
// {
//     Console.WriteLine($"Bericht ontvangen; topic={args.Topic}; message={args.Message};");
// };

// await client.SubscribeToTopic("ConsoleClient");

// while(true)
// {
//     Console.WriteLine("Voer een bericht of stop in:");
//     string message = Console.ReadLine();
//     if (message == "stop")
//     {
//         break;
//     }
//     await client.PublishMessage(message, "topicnaam");

//     Thread.Sleep(1000);
//     Console.WriteLine(",");

//     client.SubscribeToTopic("topicnaam")
// }



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
