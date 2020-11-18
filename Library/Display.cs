using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class DisplayControl : IDisplay
    {
        // Variables
        public string DisplayAreaStation { get; private set; } = "";
        public string DisplayAreaCharger { get; private set; } = "";


        public DisplayControl()
        {

        }

        public void PrintStationMsg(string Msg)
        {
            DisplayAreaStation = Msg;
            Update();
        }

        public void PrintChargerMsg(string Msg)
        {
            DisplayAreaCharger = Msg;
            Update();
        }

        public void Update()
        {
            // if (ClearConsole) Console.Clear(); // remove for debugging //cannot be run in test's, throws I/O error
            Console.WriteLine("*************** " + DateTime.Now + " ***************");
            Console.WriteLine("Station   : " + DisplayAreaStation);
            Console.WriteLine("Charger   : " + DisplayAreaCharger);
        }


    }
}
