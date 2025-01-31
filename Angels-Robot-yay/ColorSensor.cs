using Avans.StatisticalRobot;
using mySensors;
using NLog;

namespace ColorSensor_namespace;
public class ColorSensor : Sensor
{
    RGBSensor rGBSensor;

    public ColorSensor(byte pinByte) : base(pinByte)
    {
        rGBSensor = new RGBSensor(pinByte);
        rGBSensor.Begin();
        rGBSensor.SetGain(RGBSensor.Gain.GAIN_16X);
        rGBSensor.SetIntegrationTime(RGBSensor.IntegrationTime.INTEGRATION_TIME_154MS);
    }

    ushort r, g, b, c;

    new public (ushort, ushort, ushort, ushort) GetMeasurement()
    {
        rGBSensor.GetRawData(out r, out g, out b, out c);
        return (r, g, b, c);
    }

    new public string GetMeasurementAsString()
    {
        (r, g, b, c) = GetMeasurement();
        return $"r: {r}, g: {g}, b: {b}, c: {c}";
    }

    public (float, float, float, ushort) GetScaledMeasurement()
    {
        (r, g, b, c) = GetMeasurement();
        return ((float)r/c, (float)g/c, (float)b/c, c);
    }

    float rFloat, gFloat, bFloat;
    public string GetScaledMeasurementAsString()
    {
        (rFloat, gFloat, bFloat, c) = GetScaledMeasurement();
        return $"r: {rFloat}, g: {gFloat}, b: {bFloat}, c: {c}";
    }

    // The following colors are measured at 3 cm distance from the color sensor.
    (double, double, double) averageRed = (0.30076924, 0.34076923, 0.3146154);
    (double, double, double) averageYellow = (0.33738938, 0.35176992, 0.26659292);
    (double, double, double) averageGreen = (0.23525019, 0.38909635, 0.32337564);
    double scoreRed, scoreYellow, scoreGreen;
    public string ColorMatch()
    {
        (rFloat, gFloat, bFloat, c) = GetScaledMeasurement();
        Console.WriteLine(c);
        scoreRed = -Math.Abs(rFloat-averageRed.Item1)-Math.Abs(gFloat-averageRed.Item2)-Math.Abs(bFloat-averageRed.Item3);
        scoreYellow = -Math.Abs(rFloat-averageYellow.Item1)-Math.Abs(gFloat-averageYellow.Item2)-Math.Abs(bFloat-averageYellow.Item3);
        scoreGreen = -Math.Abs(rFloat-averageGreen.Item1)-Math.Abs(gFloat-averageGreen.Item2)-Math.Abs(bFloat-averageGreen.Item3);
        Console.WriteLine($"Red: {scoreRed}, Yellow: {scoreYellow}, Green: {scoreGreen}");
        if (new [] { scoreRed, scoreYellow, scoreGreen }.Max() > -0.05)
        {
            return "None";
        }
        else if ((scoreRed > scoreYellow) & (scoreRed > scoreGreen))
        {
            return "Red";
        }
        else if ((scoreGreen > scoreRed) & (scoreGreen > scoreYellow))
        {
            return "Green";
        }
        else if ((scoreYellow > scoreRed) & (scoreYellow > scoreGreen))
        {
            return "Yellow";
        }
        return "None";
    }
}