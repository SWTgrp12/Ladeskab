using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public interface IRFIDReader
    {
        event EventHandler<RfidEventArgs> RfidHandler;
        void RfidEvent(int id);
    }
}
