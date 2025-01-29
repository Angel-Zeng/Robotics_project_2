//Code voor de speaker waar ik mijn ziel aan heb uitgegeven

using System.Device.Gpio;
using System.Threading;
using Avans.StatisticalRobot;

using System.Diagnostics; // voor de stopwatch

public class Speaker
{
    private readonly int _pin;
    private readonly GpioController _gpio;

    // private readonly int[] _bassTab = { 1517, 1912 }; // e5 c5
    private readonly int[] _bassTab = { 1912, 1517, 1275, 1912, 1517, 1275, 1047}; //  C5, E5, G5, C5, E5, G5
    
    


    public Speaker(int pin)
    {
        _pin = pin;
        _gpio = new GpioController();
        _gpio.OpenPin(_pin, PinMode.Output);
        _gpio.Write(_pin, PinValue.Low);
    }

    public void PlayTone(int noteIndex)
    {
        if (noteIndex < 0 || noteIndex >= _bassTab.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(noteIndex), "Invalid note index.");
        }

        for (int i = 0; i < 100; i++)
        {
            _gpio.Write(_pin, PinValue.High);
            DelayMicroseconds(_bassTab[noteIndex]);
            _gpio.Write(_pin, PinValue.Low);
            DelayMicroseconds(_bassTab[noteIndex]);
        }
    }

    public void PlaySequence()
    {
        for (int noteIndex = 0; noteIndex < _bassTab.Length; noteIndex++)
        {
            PlayTone(noteIndex);
            Thread.Sleep(100); //pauzeduratie tussen noten
        }
    }


    private void DelayMicroseconds(int microseconds)
    {
        var stopwatch = Stopwatch.StartNew(); // Start measuring time
        long ticksToWait = microseconds * (Stopwatch.Frequency / 1_000_000); // Convert microseconds to ticks
        while (stopwatch.ElapsedTicks < ticksToWait)
        {
            // Busy-wait loop for accurate delay
        }
    }
}
