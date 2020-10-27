using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public interface IDoor
    {
        event EventHandler CloseHandler;
        event EventHandler OpenHandler;
        void Open();
        void Close();
        void Lock();
        void Unlock();
        public Door.DoorState GetDoorState();

    }
}
