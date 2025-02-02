public class BatteryService
{
    private int _millivolts; // Store the most recent millivolt reading

    public int GetBatteryPercentage()
    {
        return BerekeningBatterijPercentage(_millivolts);
    }

    public void UpdateBatteryMillivolts(int millivolts)
    {
        _millivolts = millivolts;
    }

    private int BerekeningBatterijPercentage(int millivolts)// Omrekening van de milivolts naar percentage
    {
        const int minVoltage = 1000;  //constant omdat deze niet veranderd mogen worden! 
        const int maxVoltage = 1400;

        if (millivolts <= minVoltage) return 0; //als minder of gelijk is aan 1000 dan is het gewoon leeg en geeft het 0
        if (millivolts >= maxVoltage) return 100; // als het groter of gelijk is aan 1400 dan is hij helemaal opgeladen! 

        return (millivolts - minVoltage) * 100 / (maxVoltage - minVoltage); // minimale voltage van de meting aftrekken en dan schalen 
                                                                            // met verschil van de max en min, wat in dit geval gewoon 400 is. 
    }
}




