using Avans.StatisticalRobot;
using mySensors;

public class Knoppie : Sensor
{
    Button Buttontje;

    public Knoppie(int pinPoort) : base(pinPoort)
    {
        Buttontje = new Button(pinPoort);
    }

    new public int GetMeasurement()
    {
        if (Buttontje.GetState() == "Pressed")
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    new public string GetMeasurementAsString()
    {
        return Buttontje.GetState();
    }

    bool lampjeMagTogglen = true;
    public bool ButtonPressed()
    {
        if (GetMeasurement() == 1)
        {
            if (lampjeMagTogglen) //Anders toggled hij de hele tijd zolang het knopje ingedrukt staat
            {
                lampjeMagTogglen = false;
                return true; 
            }
            return false; //nu geeft ie altijd iets terug
        }
        else
        { //Wanneer knopje wordt losgelaten kan hij weer gewisseld worden 
            lampjeMagTogglen = true; 
            return false; 
        }
    }
}