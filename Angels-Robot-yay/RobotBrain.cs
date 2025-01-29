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
//         switch(state) {
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
//         if (knoppie.ButtonPressed())
//         {
//             state = 2;
//             return;
//         }
//     }

//     private void EmergencyStop()
//     {

//     }   
// }