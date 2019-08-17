using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    interface ICommand
    {
        bool IsMatch(ControllerValues values);
        void Execute(ControllerValues values);
    }
}
