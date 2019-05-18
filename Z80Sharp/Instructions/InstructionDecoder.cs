using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp.Instructions
{
    public class InstructionDecoder
    {
        public static int UnimplementedOpcode(IZ80CPU cpu, byte[] instruction)
        {
            throw new NotImplementedException();
        }
    }
}
