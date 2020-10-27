using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RfidEventArgs> RfidHandler;
        /// raises the event with an event argument containing the id.
        public void RfidEvent(int id)
        {
            // creates event arguments containing the id
            RfidEventArgs eventboy = new RfidEventArgs(id);
            // invoking the event with the EventHandler
            RfidHandler?.Invoke(this, eventboy);
        }
    }
}
public class RfidEventArgs : EventArgs
{
    public RfidEventArgs(int id)
    {
        id_ = id;
    }
    public int id_ { get; set; }
}
