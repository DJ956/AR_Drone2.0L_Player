using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace AR_Drone2._0L_Player
{
    public class GameController
    {
        private List<DeviceInstance> directInputs;
        private DirectInput directInput;
        public Joystick GamePad { get; set; }
        public JoystickState GamePadState { get; set; }
        public bool[] buttons { get; set; }


        public GameController()
        {
            directInputs = new List<DeviceInstance>();
            directInput = new DirectInput();

            directInputs.AddRange(directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly));
            GamePad = new Joystick(directInput, directInputs[0].InstanceGuid);
        }

        private int Map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        //未入力:-1
        //□:0
        //○:2
        //△:3
        //×:1
        //R1:5
        //R2:7
        //R3:11
        //L1:4
        //L2:6
        //L3:10
        //option:9
        //PS:12
        //Share:8

        //RotationZ 右アナログスティックの値
        //X 左アナログの値
        //Y 左アナログの値
        //Z 右アナログの値
        public void UpdateGamePad()
        {
            if (GamePad.Acquire().IsFailure) return;
            if (GamePad.Poll().IsFailure) return;
            if (SlimDX.Result.Last.IsFailure) return;

            GamePadState = GamePad.GetCurrentState();

            buttons = GamePadState.GetButtons();
            Console.WriteLine("ForceX:" + GamePadState.X);
            Console.WriteLine("ForceY:" + GamePadState.Y);
            Console.WriteLine("ForceZ:" + GamePadState.Z);
            Console.WriteLine("ForceZ2:" + GamePadState.RotationZ);
            Console.WriteLine("Buttons:" + buttons.ToList().FindIndex(b => b == true));
            Console.WriteLine("-----------------------------------");
            GamePad.Unacquire();
        }

        public void Dispose()
        {
            GamePad.Unacquire();
            GamePad.Dispose();
        }
    }
}
