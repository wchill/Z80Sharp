using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public enum Z80InterruptMode
    {
        External = 0,
        FixedAddress = 1,
        Vectorized = 2
    }
}
