// Dit is voorbeeldcode uit de Avans Library

using Avans.StatisticalRobot;
using Avans.StatisticalRobot.Interfaces;


public class RobotRijden : IUpdatable
{
    private double speedStep = 0.01; // Acceleratie

    public double SpeedStep {
        get
        {
            return speedStep;
        }

        set
        {
            if (value > 0.0 && value <= 1.0)
            {
                speedStep = value;
            }
        }
    }

    private double targetSpeed = 0.0; // Snelheid   -1.0   0.0   1.0 

    public double TargetSpeed {
        get
        {
            return targetSpeed;
        }
        
        set
        {
            if (value >= -1.0 && value <= 1.0)
            {
                targetSpeed = value;
            }

        }
    }

    private double actualSpeed; //gelezen waarde

    public double ActualSpeed {
        get { return actualSpeed; }
    }

    //om te kijken of motors rijden
    public bool DriveActive { get; set; } = true;

    public RobotRijden()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
    }

    
    // zet om naar een waarde die de robot begrijpt, -300 tot 300
    private short ToRobotSpeedValue(double speed)
    {
        return (short) Math.Round(speed * 300.0);
    }

    
    private void ControlRobotMotorSpeeds()
    {
        if (DriveActive)
        {
            // heb even deze waardes op negatief gezet, sensoren verkeerd gezet
            Robot.Motors(
                ToRobotSpeedValue(-actualSpeed),
                ToRobotSpeedValue(-actualSpeed)
            );
        }
    }


    public void EmergencyStop()
    {
        targetSpeed = 0.0;
        actualSpeed = 0.0;
        ControlRobotMotorSpeeds();
    }

    //hier en daar aanroepen
    public void Update()
    {
        bool motorUpdate = actualSpeed != targetSpeed; 
        if (actualSpeed < targetSpeed)
        {
            
            actualSpeed += speedStep;
            if (actualSpeed > 1.0)
            {
                actualSpeed = 1.0;
            }
            else if (actualSpeed > targetSpeed)
            {
                actualSpeed = targetSpeed;
            }
        }
        else if (actualSpeed > targetSpeed)
        {
            
            actualSpeed -= speedStep;
            if (actualSpeed < -1.0)
            {
                actualSpeed = -1.0;
            }
            else if (actualSpeed < -targetSpeed)
            {
                actualSpeed = -targetSpeed;
            }
        }

        Console.WriteLine($"DEBUG: Target speed {targetSpeed}, actual speed {actualSpeed}");
        
        // verzenden van nieuwe snelheid naar robot
        
        if (motorUpdate)
        {
            ControlRobotMotorSpeeds();
        }
    }
}