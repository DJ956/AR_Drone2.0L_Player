using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class ControllerNofFoundException : Exception
    {
        public ControllerNofFoundException() : base() { }
        public ControllerNofFoundException(string message) : base(message) { }
    }
}
