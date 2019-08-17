using System;
using System.Collections.Generic;
using System.Linq;
using SlimDX.DirectInput;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class GameController
    {
        private List<DeviceInstance> directInputs;
        private DirectInput directInput;

        public Joystick GamePad { get; set; }
        public JoystickState GamePadState { get; set; }

        public List<ControllerCommand> Commands { get; private set; }

        public GameController(Action<ControllerValues> onTakeOff, Action<ControllerValues> onLand,
            Action<ControllerValues> onHover, Action<ControllerValues> onUp, Action<ControllerValues> onDown,
            Action<ControllerValues> onLeft, Action<ControllerValues> onRight,
            Action<ControllerValues> onForward, Action<ControllerValues> onBack,
            Action<ControllerValues> onTurnLeft, Action<ControllerValues> onTurnRight,
            Action<ControllerValues> onFlatTrim)
        {
            directInputs = new List<DeviceInstance>();
            directInput = new DirectInput();

            directInputs.AddRange(directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly));
            GamePad = new Joystick(directInput, directInputs[0].InstanceGuid);

            Commands = new List<ControllerCommand>();
            Commands.Add(new TakeOffCommand(onTakeOff));
            Commands.Add(new LandCommand(onLand));
            Commands.Add(new HoverCommand(onHover));
            Commands.Add(new UpCommand(onUp));
            Commands.Add(new DownCommand(onDown));
            Commands.Add(new LeftCommand(onLeft));
            Commands.Add(new RightCommand(onRight));
            Commands.Add(new ForwardCommand(onForward));
            Commands.Add(new BackCommand(onBack));
            Commands.Add(new TurnLeftCommand(onTurnLeft));
            Commands.Add(new TurnRightCommand(onTurnRight));
            Commands.Add(new FlatTrimCommand(onFlatTrim));
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


        public bool TryUpdateGamePad(ref ControllerValues values)
        {
            if (GamePad.Acquire().IsFailure) return false;
            if (GamePad.Poll().IsFailure) return false;
            if (SlimDX.Result.Last.IsFailure) return false;

            GamePadState = GamePad.GetCurrentState();

            values.X = GamePadState.X;
            values.Y = GamePadState.Y;
            values.LeftZ = GamePadState.Z;
            values.RightZ = GamePadState.RotationZ;
            values.ButtonIndex = GamePadState.GetButtons().ToList().FindIndex(b => b == true);

            GamePad.Unacquire();

            return true;
        }

        public void Dispose()
        {
            GamePad.Unacquire();
            GamePad.Dispose();
        }
    }
}
