using System.Device.Gpio;
using Avans.StatisticalRobot;
using mySensors;
using SimpleMqtt;
using System.Diagnostics;
using System.Device.I2c;

// IRSensor iRSensor = new IRSensor(22);
// while (true)
// {
//     Robot.Wait(1000);
//     int measurement = iRSensor.GetMeasurement();
//     Console.WriteLine(measurement);
// }

/* Kleursensor p1, chaos */
// ColorSensor colorSensor = new ColorSensor(0x29);

// while (true)
// {
//     Robot.Wait(1000);
//     Byte state = colorSensor.GetColor();

// }


/////////////Below code works for Speaker, keep it here (ELOY)
// Speaker speaker = new Speaker(26);

// Console.WriteLine("Muziekje spelen...");
// speaker.PlaySequence();
/////////////////


// Console.WriteLine("Klaar met spelen.");

RobotRijden driveSystem = new RobotRijden();
driveSystem.TargetSpeed = 0.2;

Knoppie knoppie = new Knoppie(6);

Led led5 = new Led(5);
led5.SetOff();

AfstandsSensor afstandsSensor = new AfstandsSensor(16);


// // MQTT
SimpleMqttClient mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("Robot");

mqttClient.OnMessageReceived += (sender, args) =>
{
    Console.WriteLine($"Bericht ontvangen; topic={args.Topic}; message={args.Message};");
    driveSystem.EmergencyStop();
};

await mqttClient.SubscribeToTopic("tasks");

// //MQTT



// while (true)
// {
//     await mqttClient.PublishMessage($"{Robot.ReadBatteryMillivolts()}", "sensordata");
// }

while(true)
{
    if (knoppie.ButtonPressed())
    {
        driveSystem.EmergencyStop();
    }


    if (afstandsSensor.BotsingsGevaar())
    {
        driveSystem.EmergencyStop();
        // speaker.PlaySequence();
        while (!afstandsSensor.VeiligeAfstand())
        {
            Robot.Wait(100);
        }
        driveSystem.TargetSpeed = 0.2;
    }

    driveSystem.Update();
    await mqttClient.PublishMessage($"{Robot.ReadBatteryMillivolts()}", "sensordata");
    Robot.Wait(100);

    

}

Knoppie mijnKnoppie = new Knoppie(6);

mijnKnoppie.GetMeasurement();


// bool ledIsOn = false;
// bool lampjeMagTogglen = true; //Om te kijken of het lampje mag wisselen van aan naar uit of andersom



// static bool toggleLed(bool ledIsOn, Led lampie)
// {
//     if (ledIsOn)
//     {
//         lampie.SetOff();
//         ledIsOn = false;
//     }
//     else
//     {
//         lampie.SetOn();
//         ledIsOn = true;
//     }
//     return ledIsOn;
// }

// while (true)
// {
//     Console.WriteLine("Het aantal millivolts :" + Robot.ReadBatteryMillivolts());
//     Robot.Wait(2000);

//     Robot.Wait(10);


//     Console.WriteLine(button6.GetState()); 
// }
