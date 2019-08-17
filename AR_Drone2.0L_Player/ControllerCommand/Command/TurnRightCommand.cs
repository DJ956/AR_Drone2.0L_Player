using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class TurnRightCommand : ControllerCommand
    {
        public static readonly int MIN = 33000;
        public static readonly int MAX = 65535;

        public TurnRightCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            if(values.LeftZ >= MIN && values.LeftZ <= MAX) { return true; }
            return false;
        }
    }
}
