using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class UpCommand : ControllerCommand
    {
        private static int BUTTON_INDEX = 6;

        public UpCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            return BUTTON_INDEX == values.ButtonIndex;
        }
    }
}
