using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    interface IDoor
    {
        event EventHandler CloseHandler;
        event EventHandler OpenHandler;
        void Open();
        void Close();
        void Lock();
        void Unlock();


    }
}
