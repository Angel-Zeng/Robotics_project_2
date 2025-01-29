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

int state;
state = 0;
bool isDriving = false; // To track whether the robot is driving or not

RobotRijden driveSystem = new RobotRijden();
driveSystem.TargetSpeed = 0.2;

Knoppie knoppie = new Knoppie(6);

Led led5 = new Led(5);
led5.SetOff();

AfstandsSensor afstandsSensor = new AfstandsSensor(16);


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
            driveSystem.EmergencyStop();
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

    }
    Robot.Wait(100);
}

void DriveForward()
{

    driveSystem.TargetSpeed = 0.2;
    driveSystem.Update();

    if (afstandsSensor.BotsingsGevaar())
    {
        state = 3;
    }

    if (knoppie.ButtonPressed())
        {
            state = 2;
        }
}

void EmergencyStop()
{
    driveSystem.EmergencyStop();
    if (knoppie.ButtonPressed())
        {
            state = 1;
        }
}

void CollisionStop()
{
    driveSystem.EmergencyStop();
    if (afstandsSensor.VeiligeAfstand())
    {
        state = 1;
    }

    if (knoppie.ButtonPressed())
        {
            state = 2;
        }
}

