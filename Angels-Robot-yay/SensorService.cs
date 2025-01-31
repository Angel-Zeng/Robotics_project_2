// using System.Collections.Concurrent;

// public class SensorService : IDisposable
// {
//     private readonly ColorSensor _colorSensor;
//     private CancellationTokenSource _cancellationTokenSource;
//     private readonly ConcurrentQueue<string> _colorQueue;

//     public event Action<string>? OnColorScanned;

//     public SensorService(byte pin)
//     {
//         _colorSensor = new ColorSensor(pin);
//         _cancellationTokenSource = new CancellationTokenSource();
//         _colorQueue = new ConcurrentQueue<string>();
//     }

//     public void StartScanning()
//     {
//         Task.Run(async () =>
//         {
//             while (!_cancellationTokenSource.IsCancellationRequested)
//             {
//                 var colorName = _colorSensor.ColorMatch();

//                 // Map detected colors to hex values for the UI
//                 var colorHex = colorName switch
//                 {
//                     "Red" => "#D32F2F",
//                     "Yellow" => "#FBC02D",
//                     "Green" => "#388E3C",
//                     _ => "#bbb" // Default grey for none
//                 };

//                 _colorQueue.Enqueue(colorHex);

//                 OnColorScanned?.Invoke(colorHex);

//                 await Task.Delay(1000);
//             }
//         });
//     }

//     public void StopScanning() => _cancellationTokenSource.Cancel();

//     public void Dispose()
//     {
//         StopScanning();
//         _colorSensor.Dispose();
//     }
// }
