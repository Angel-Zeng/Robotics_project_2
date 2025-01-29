using Avans.StatisticalRobot;
using mySensors;

public class AfstandsSensor : Sensor
{
    const int AfstandsLimiet = 10; 
    public bool BotsingsGevaar()
    {
        if (GetMeasurement() < AfstandsLimiet)
        {
            return true; 
        }
        else 
        {
            return false;
        }
    }

    public bool VeiligeAfstand()
    {
        if (GetMeasurement() > AfstandsLimiet+5)
        {
            return true; 
        }
        else 
        {
            return false;
        }
    }
    Ultrasonic ultrasonic;

    public AfstandsSensor(int pinPoort) : base(pinPoort)
    {
        ultrasonic = new Ultrasonic(pinPoort);
    }
    bool afterFirstMeasurement = false;
    new public int GetMeasurement()
    {
        int afstand = ultrasonic.GetUltrasoneDistance();
        Console.WriteLine($"DEBUG: Measured distance: {afstand} cm");
        
        if (afterFirstMeasurement)
        {
            return afstand; 
        }
        else
        {
            afterFirstMeasurement = true;
            return 100; // de default return voor de eerste meting 
        }
    }

    new public string GetMeasurementAsString()
    {
        return GetMeasurement().ToString();
    }

}