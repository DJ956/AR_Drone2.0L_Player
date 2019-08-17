using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public abstract class ControllerCommand : ICommand
    {
        protected Action<ControllerValues> ActionCmd;

        public ControllerCommand(Action<ControllerValues> action)
        {
            ActionCmd = action;
        }

        public void Execute(ControllerValues values)
        {
            ActionCmd(values);
        }

        public abstract bool IsMatch(ControllerValues values);

        
    }
}
