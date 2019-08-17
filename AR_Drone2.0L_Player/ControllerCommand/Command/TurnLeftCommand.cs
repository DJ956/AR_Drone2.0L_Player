using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class TurnLeftCommand : ControllerCommand
    {
        public static readonly int MIN = 28000;
        public static readonly int MAX = 0;

        public TurnLeftCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            if(values.LeftZ <= MIN && values.LeftZ >= MAX) { return true; }
            return false;
        }
    }
}
