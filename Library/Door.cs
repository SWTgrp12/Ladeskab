using System;
using System.Collections.Generic;
using System.Text;

namespace afl2
{
    public class Door
    {
        public void Lock()
        {
            // must send doorclosed event to StationControl

        }

        public void Unlock()
        {
            // must send dooropened event to StationControl
        }
    }
}
