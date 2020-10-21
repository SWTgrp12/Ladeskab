using System;
using System.Collections.Generic;
using System.Text;


namespace Display
{
    public interface IDisplay
    {
        string DisplayAreaStation { get; }
        string DisplayAreaCharger { get; }
        //bool ClearConsoleWhenUpdating { get; set; }

        void PrintStationMsg(string Msg);
        void PrintChargerMsg(string Msg);

        void Update();  // writes the current display's again



    }
}