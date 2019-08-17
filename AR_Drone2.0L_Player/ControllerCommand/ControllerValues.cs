using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class ControllerValues
    {
        public int ButtonIndex { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int LeftZ { get; set; }
        public int RightZ { get; set; }

        public ControllerValues(int buttonIndex, int x, int y, int leftz, int rightz)
        {
            ButtonIndex = buttonIndex;
            X = x;
            Y = y;
            LeftZ = leftz;
            RightZ = rightz;
        }

        public void Clear()
        {
            ButtonIndex = -2;
            X = -1;
            Y = -1;
            LeftZ = -1;
            RightZ = -1;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{LeftZ} Z2:{RightZ} BtnIdx:{ButtonIndex}";
        }
    }
}
