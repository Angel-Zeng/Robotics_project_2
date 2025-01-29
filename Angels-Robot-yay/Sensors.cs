using Avans.StatisticalRobot;

namespace mySensors;


//standaardklasse voor sensoren
public class Sensor
{
    private int pinId;

    public Sensor(int pinPoort)
    {
        pinId = pinPoort;
    }

    public int GetMeasurement()
    {
        return -1;
    }

    public string GetMeasurementAsString()
    {
   
        return "hihi haha";
    }
    
}


// public class Knoppie : Sensor
// {
//     Button Buttontje;

//     public Knoppie(int pinPoort) : base(pinPoort)
//     {
//         Buttontje = new Button(pinPoort);
//     }
//     new public int GetMeasurement()
//     {
//         return 1;
//     }
// }
