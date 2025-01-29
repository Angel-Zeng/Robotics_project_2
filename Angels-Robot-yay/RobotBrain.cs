// public class RobotBrain
// {
//     private Knoppie knoppie;

//     public RobotBrain(Knoppie _knoppie)
//     {
//         knoppie = _knoppie;
//     }

//     public int state;
//     public void RunState()
//     {
//         switch (state)
//         {
//             case 0:
//                 state = 1;
//                 break;
//             case 1:
//                 DriveForward();
//                 break;
//             case 2:
//                 EmergencyStop();
//                 break;
//             case 3:

//         }
//     }

//     private void DriveForward()
//     {
//         AfstandsSensor afstandsSensor = new AfstandsSensor(16);

//         RobotRijden driveSystem = new RobotRijden();
//         driveSystem.TargetSpeed = 0.2;
//         driveSystem.Update();

//         if (afstandsSensor.BotsingsGevaar())
//         {
//             driveSystem.EmergencyStop();
//             // speaker.PlaySequence();
//             while (!afstandsSensor.VeiligeAfstand())
//             {
//                 Robot.Wait(100);
//             }
//             driveSystem.TargetSpeed = 0.2;
//         }


//         // if (knoppie.ButtonPressed())
//         // {
//         //     state = 2;
//         //     return;
//         // }
//     }

//     private void EmergencyStop()
//     {

//     }
// }