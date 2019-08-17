﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    public class LeftCommand : ControllerCommand
    {
        public static readonly int MIN = 28000;
        public static readonly int MAX = 0;

        public LeftCommand(Action<ControllerValues> action) : base(action)
        {

        }

        public override bool IsMatch(ControllerValues values)
        {
            if(values.X <= MIN && values.X >= MAX) { return true; }
            return false;
        }
    }
}
