using Newtonsoft.Json;
using SimpleMqtt;

public class MqttData
{
    public List<string> Colors { get; set; }
    public bool IsOn { get; set; }
    public int BatteryMilliVoltage { get; set; }
    public int DistanceSensor { get; set; }
}

public class MqttClass
{
    private SimpleMqttClient _mqttConnection;
    public MqttClass()
    {
        // hier maak ik dus de connectie met de MQTT server.
        this._mqttConnection = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("client-Id");


        // hier moet je ook de topics subscriben.
        // wanneer er een bericht binnen komt run je IkHebEenBerichtOntvangen()
        this._mqttConnection.OnMessageReceived += (sender, args) =>
        {
            
            IkHebEenBerichtOntvangen(args.Message, args.Topic);
        };

        
    }

    public async Task StartListeningToMqtt() {
        await this._mqttConnection.SubscribeToTopic("tasks");
    }

    public async Task FormatMessage(MqttData data)
    {
        // hier format ik de data naar een string.
        string StringifiedData = JsonConvert.SerializeObject(data, Formatting.Indented);
        Console.WriteLine(StringifiedData);
        await this.SendData($"{StringifiedData}");
    }

    public async Task SendData(string textBericht)
    {
        // hier stuur ik de data naar de MQTT server.
        await this._mqttConnection.PublishMessage(textBericht, "tasks");
    }

    private void IkHebEenBerichtOntvangen(string text, string topic)
    {
        Console.WriteLine($"[Received] Topic: {topic}, Message: {text}");
        // vanuit hier stuur ik dingen aan naar mijn andere classes.
        Console.WriteLine($"Received message: {text}");
        var data = JsonConvert.DeserializeObject<MqttData>(text);
        if (data == null)
        {
            Console.WriteLine("No good data received");
            return;
        }
        // hier kan je dan de data gebruiken.
        // hier wordt het weer omgezet als MqttData object.

    }

    public void stopConnectie() {
        this._mqttConnection.Dispose();
    }
}