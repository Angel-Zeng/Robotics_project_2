using System.Device.Gpio;
using Avans.StatisticalRobot;
using mySensors;
using SimpleMqtt;
using System.Diagnostics;
using System.Device.I2c;
using System.Runtime.InteropServices;
using ColorSensor_namespace;

// //Kleursensor
// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton<SensorService>(provider => new SensorService(0x29));

// var app = builder.Build();
// app.MapBlazorHub();
// app.MapFallbackToPage("/_Host");
// app.Run();
// //Kleursensor

// IRSensor iRSensor = new IRSensor(22);
// while (true)
// {
//     Robot.Wait(1000);
//     int measurement = iRSensor.GetMeasurement();
//     Console.WriteLine(measurement);
// }


SimpleMqttClient mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ("Robot");

/* Kleursensor p1, chaos */
ColorSensor colorSensor = new ColorSensor(0x29);

int BerekeningBatterijPercentage(int millivolts) // Omrekening van de milivolts naar percentage
{
    const int minVoltage = 1000;  //constant omdat deze niet veranderd mogen worden! 
    const int maxVoltage = 1400; 

    if (millivolts <= minVoltage) return 0; //als minder of gelijk is aan 1000 dan is het gewoon leeg en geeft het 0
    if (millivolts >= maxVoltage) return 100; // als het groter of gelijk is aan 1400 dan is hij helemaal opgeladen! 

    return (millivolts - minVoltage) * 100 / (maxVoltage - minVoltage); // minimale voltage van de meting aftrekken en dan schalen 
    // met verschil van de max en min, wat in dit geval gewoon 400 is. 
}

while (true)
{
    Robot.Wait(1000);
    string color = colorSensor.ColorMatch();
    Console.WriteLine(color);

    if (color != "None")
    {
        await mqttClient.PublishMessage($"{color}", "RGB");
    }

    int millivolts = Robot.ReadBatteryMillivolts();
    int batteryPercentage = BerekeningBatterijPercentage(millivolts);


    // await mqttClient.PublishMessage($"{Robot.ReadBatteryMillivolts()}", "sensordata"); 

    await mqttClient.PublishMessage($"{batteryPercentage}%", "sensordata");
    // Console.WriteLine($"Batterij Voltage: {millivolts}mV ({batteryPercentage}%)");

}




/////////////Below code works for Speaker, keep it here (ELOY)
// Speaker speaker = new Speaker(26);

// Console.WriteLine("Muziekje spelen...");
// speaker.PlaySequence();
/////////////////


// Console.WriteLine("Klaar met spelen.");

int state = 0;
bool isDriving = false; // To track whether the robot is driving or not

RobotRijden driveSystem = new RobotRijden();
// driveSystem.TargetSpeed = 0.2;

Knoppie knoppie = new Knoppie(6);

Led led5 = new Led(5);
led5.SetOff();

AfstandsSensor afstandsSensor = new AfstandsSensor(16);

IRSensor iRSensor = new IRSensor(22);
// ColorSensor colorSensor = new ColorSensor(0x29);

DateTime? turnStartTime = null;
double defaultStartTurnTime = 2.0;
TimeSpan lastTurnTime = TimeSpan.FromSeconds(defaultStartTurnTime);

while (true)
{
    RunState();
}

void RunState()
{
    Console.WriteLine($"Current state : {state}");
    switch (state)
    {
        case 0:
            state = 1;
            // driveSystem.EmergencyStop();
            break;
        case 1:
            DriveForward();
            break;
        case 2:
            EmergencyStop();
            break;
        case 3:
            CollisionStop();
            break;
        case 5:
            TurnLeft();
            break;
        case 6:
            TurnRight();
            break;
        case 99:
            TestIR();
            // TestColor();
            break;

    }
    Robot.Wait(100);
}

void DriveForward()
{

    driveSystem.TargetSpeedLeft = 0.2;
    driveSystem.TargetSpeedRight = 0.2;
    driveSystem.Update();

    if (afstandsSensor.BotsingsGevaar())
    {
        state = 3;
        return;
    }

    if (knoppie.ButtonPressed())
    {
        state = 2;
        return;
    }

    if (iRSensor.GetMeasurement() == 0)
    {
        state = 5;
        return;
    }

}

void TurnLeft()
{
    if (turnStartTime == null)
    {
        driveSystem.EmergencyStop();
        turnStartTime = DateTime.UtcNow;
    }
    driveSystem.TargetSpeedLeft = 0.2;
    driveSystem.TargetSpeedRight = -0.2;
    driveSystem.Update();

    if (knoppie.ButtonPressed())
    {
        state = 2;
        turnStartTime = null;
        lastTurnTime = TimeSpan.FromSeconds(defaultStartTurnTime);
        return;
    }
    if (iRSensor.GetMeasurement() == 1)
    {
        driveSystem.EmergencyStop();
        state = 1;
        turnStartTime = null;
        lastTurnTime = TimeSpan.FromSeconds(defaultStartTurnTime);
    }
    if (DateTime.UtcNow > turnStartTime?.Add(lastTurnTime))
    {
        state = 6;
        turnStartTime = null;
        lastTurnTime += TimeSpan.FromSeconds(defaultStartTurnTime * 1.5);
        return;
    }

    if (knoppie.ButtonPressed())
    {
        state = 2;
        return;
    }
}

void TurnRight()
{
    if (turnStartTime == null)
    {
        driveSystem.EmergencyStop();
        turnStartTime = DateTime.UtcNow;
    }
    driveSystem.TargetSpeedLeft = -0.2;
    driveSystem.TargetSpeedRight = 0.2;
    driveSystem.Update();

    if (knoppie.ButtonPressed())
    {
        state = 2;
        turnStartTime = null;
        lastTurnTime = TimeSpan.FromSeconds(defaultStartTurnTime);
        return;
    }
    if (iRSensor.GetMeasurement() == 1)
    {
        driveSystem.EmergencyStop();
        state = 1;
        turnStartTime = null;
        lastTurnTime = TimeSpan.FromSeconds(defaultStartTurnTime);
    }
    if (DateTime.UtcNow > turnStartTime?.Add(lastTurnTime))
    {
        state = 5;
        turnStartTime = null;
        lastTurnTime += TimeSpan.FromSeconds(defaultStartTurnTime * 1.5);
        return;
    }

    if (knoppie.ButtonPressed())
    {
        state = 2;
        return;
    }


}

void EmergencyStop()
{
    driveSystem.EmergencyStop();
    if (knoppie.ButtonPressed())
    {
        state = 1;
        return;
    }
}

void CollisionStop()
{
    driveSystem.EmergencyStop();
    if (afstandsSensor.VeiligeAfstand())
    {
        state = 1;
        return;
    }

    if (knoppie.ButtonPressed())
    {
        state = 2;
        return;
    }
}

void TestIR()
{
    int measurement = iRSensor.GetMeasurement();
    Console.WriteLine(measurement);
}

// void TestColor()
// {
//     Console.WriteLine(colorSensor.GetScaledMeasurementAsString());    
//     Console.WriteLine(colorSensor.ColorMatch());
// }
