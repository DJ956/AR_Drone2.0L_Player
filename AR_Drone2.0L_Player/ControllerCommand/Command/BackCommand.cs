using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class BackCommand : ControllerCommand
    {
        public static int MIN = 33000;
        public static int MAX = 65535;

        public BackCommand(Action<ControllerValues> action) : base(action)
        {
        }

        public override bool IsMatch(ControllerValues values)
        {
            if(values.Y >= MIN && values.Y <= MAX) { return true; }
            return false;
        }
    }
}
