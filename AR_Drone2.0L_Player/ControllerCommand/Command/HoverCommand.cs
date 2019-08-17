using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class HoverCommand : ControllerCommand
    {
        private static readonly int BUTTON_INDEX = 0;

        public HoverCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            return BUTTON_INDEX == values.ButtonIndex;
        }
    }
}
