
// public class MqttMessagesProcessingService : IHostedService
// {
//     public Task StartAsync(CancellationToken cancellationToken)
//     {
//         throw new NotImplementedException();
//     }

//     public Task StopAsync(CancellationToken cancellationToken)
//     {
//         throw new NotImplementedException();
//     }
// }

using SimpleMqtt; 
public class MqttMessageProcessingService : IHostedService
{
    private readonly IUserRepository _userRepository;
    private readonly SimpleMqttClient _mqttClient;

    public MqttMessageProcessingService(IUserRepository userRepository, SimpleMqttClient mqttClient)
    {
      	_userRepository = userRepository;  
      	_mqttClient = mqttClient;
        
        _mqttClient.OnMessageReceived += (sender, args) => {
            Console.WriteLine($"Incoming MQTT message on {args.Topic}:{args.Message}");
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
       await _mqttClient.SubscribeToTopic("#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}