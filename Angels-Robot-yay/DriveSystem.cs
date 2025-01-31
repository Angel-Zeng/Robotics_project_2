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

    private double targetSpeedLeft = 0.0; // Snelheid   -1.0   0.0   1.0 
    private double targetSpeedRight = 0.0; // Snelheid   -1.0   0.0   1.0 

    public double TargetSpeedLeft {
        get
        {
            return targetSpeedLeft;
        }
        
        set
        {
            if (value >= -1.0 && value <= 1.0)
            {
                targetSpeedLeft = value;
            }

        }
    }

    public double TargetSpeedRight {
        get
        {
            return targetSpeedRight;
        }
        
        set
        {
            if (value >= -1.0 && value <= 1.0)
            {
                targetSpeedRight = value;
            }

        }
    }
    private double actualSpeedLeft; //gelezen waarde
    private double actualSpeedRight; //gelezen waarde

    public double ActualSpeedLeft {
        get { return actualSpeedLeft; }
    }
    public double ActualSpeedRight {
        get { return actualSpeedRight; }
    }

    //om te kijken of motors rijden
    public bool DriveActive { get; set; } = true;

    public RobotRijden()
    {
        targetSpeedLeft = 0.0;
        targetSpeedRight = 0.0;
        actualSpeedLeft = 0.0;
        actualSpeedRight = 0.0;
    }

    
    // zet om naar een waarde die de robot begrijpt, -300 tot 300
    private short ToRobotSpeedValue(double speed)
    {
        return (short) Math.Round(speed * 300.0);
    }

    double speedFactorLeft = 1;
    double speedFactorRight = 0.81; // Dit zorgt ervoor dat de robot een stuk rechter rijdt.
    private void ControlRobotMotorSpeeds()
    {
        if (DriveActive)
        {
            // heb even deze waardes op negatief gezet, sensoren verkeerd gezet
            
            Robot.Motors(
                ToRobotSpeedValue(-actualSpeedLeft*speedFactorLeft),
                ToRobotSpeedValue(-actualSpeedRight*speedFactorRight)
            );
        }
    }


    public void EmergencyStop()
    {
        targetSpeedLeft = 0.0;
        targetSpeedRight = 0.0;
        actualSpeedLeft = 0.0;
        actualSpeedRight = 0.0;
        ControlRobotMotorSpeeds();
    }

    private double calculateUpdateSpeed(double actualSpeed, double targetSpeed)
    {
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
            else if (actualSpeed < targetSpeed)
            {
                actualSpeed = targetSpeed;
            }
        }
        return actualSpeed;
    }

    //hier en daar aanroepen
    public void Update()
    {
        bool motorUpdate = (actualSpeedLeft != targetSpeedLeft) || (actualSpeedRight != targetSpeedRight); 
        actualSpeedLeft = calculateUpdateSpeed(actualSpeedLeft, targetSpeedLeft);
        actualSpeedRight = calculateUpdateSpeed(actualSpeedRight, targetSpeedRight);

        Console.WriteLine($"DEBUG: Target speed left {targetSpeedLeft}, actual speed left {actualSpeedLeft}");
        Console.WriteLine($"DEBUG: Target speed right {targetSpeedRight}, actual speed right {actualSpeedRight}");
        
        // verzenden van nieuwe snelheid naar robot
        
        if (motorUpdate)
        {
            ControlRobotMotorSpeeds();
        }
    }
}