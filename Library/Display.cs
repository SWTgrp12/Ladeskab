using System;
using System.Collections.Generic;
using System.Text;

namespace afl2
{
    class Display
    {
        public void print(string message)
        {
            Console.WriteLine("----------------");
            Console.WriteLine(message);
            Console.WriteLine("----------------");
        }
    }
}
