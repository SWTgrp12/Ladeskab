using Ladeskab;
using System;
using System.Collections.Generic;
using System.Text;

namespace Display
{
    class DisplayControl : IDisplay
    {
        // Variables
        public string DisplayAreaStation { get; private set; } = "";
        public string DisplayAreaCharger { get; private set; } = "";
        private int _constructerCounter;
        // Objects
        // private IChargeControl _chargeControl;
        // private IStationControl _stationControl;


        public DisplayControl()
        {
            _constructerCounter += 1;
            Console.WriteLine("Display Constructor times called: " + _constructerCounter);
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
