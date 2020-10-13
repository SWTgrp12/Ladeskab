using Library;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Library
{
    public class Door :IDoor
    {
        public event EventHandler CloseHandler;
        public event EventHandler OpenHandler;

        // Door Has 3 different state
        private enum DoorState
        {
            Locked,
            Closed,
            Open
        };

        private DoorState _state;


        // Constructor
        public Door()
        {
            _state = DoorState.Closed;
        }
        // Checks if doorState is closed, so it can be opened
        public void Open()
        {
            Console.WriteLine("Door Opening");
            if (_state == DoorState.Closed)
            {
                _state = DoorState.Open;
                // invoking an event to send the information to StationControl
                OpenHandler?.Invoke(this, EventArgs.Empty);
                Console.WriteLine("Door Is now Open");
            }
            else
            {
                Console.WriteLine("Could not Open door: ", _state);
            }
        }
            // Checks if doorState is open, so it can be closed
        public void Close()
        {
            Console.WriteLine("Door Closing");
            if (_state == DoorState.Open)
            {
                _state = DoorState.Closed;
                CloseHandler?.Invoke(this, EventArgs.Empty);
                Console.WriteLine("Door Is now Closed");
            }
            else
            {
                Console.WriteLine("Could not Close door: ", _state);
            }

        }

        // Checks if doorState is closed, so it can be locked
        public void Lock()
        {
            Console.WriteLine("Locking Door");
            // must send doorclosed event to StationControl
            if (_state == DoorState.Closed)
            {
                _state = DoorState.Locked;
                Console.WriteLine("Door Is now Locked");
            }
            else
            {
                Console.WriteLine("Could not Lock door: ", _state);
            }
        }
        // Checks if doorState is locked, so it can be unlocked
        public void Unlock()
        {
            Console.WriteLine("Unlocking Door");
            // must send dooropened event to StationControl
            if (_state == DoorState.Locked)
            {
                _state = DoorState.Closed;
                Console.WriteLine("Door Is now Unlocked");
            }
            else
            {
                Console.WriteLine("Could not Unlock door: ", _state);
            }
        }
    }
}

