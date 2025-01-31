using Avans.StatisticalRobot;
using mySensors;

public class IRSensor : Sensor
{
    InfraredReflective infraredReflective;

    public IRSensor(int pinPoort) : base(pinPoort)
    {
        infraredReflective = new InfraredReflective(pinPoort);
    }

    int lastMeasurement = 0; //Default om te kunnen returnen in de if, voor de zekerheid
    new public int GetMeasurement()
    {
        int measurement = infraredReflective.Watch(); //Opslaan om maar 1 keer te hoeven aanroepen 
        if (measurement == -1)
        {
            Console.WriteLine("Refreshed too soon!");
            return lastMeasurement; //default is 0
        }
        else if (measurement == 0) //we draaien hiermee de measurement om van 0 naar 1 en andersom
        {
            lastMeasurement = 0; //updaten van de laatst gemeten waarde uit sensor
            return 0;
        }
        else {
            lastMeasurement = 1; //updaten van de laatst gemeten waarde uit sensor
            return 1;
        }
    }

    new public string GetMeasurementAsString()
    {
        return GetMeasurement().ToString();
    }
}