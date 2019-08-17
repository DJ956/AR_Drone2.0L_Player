using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class TakeOffCommand : ControllerCommand
    {
        private static readonly int BUTTON_INDEX = 3;

        public TakeOffCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            return BUTTON_INDEX == values.ButtonIndex;
        }
    }
}
