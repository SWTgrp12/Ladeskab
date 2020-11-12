using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    class LogControl
    {

        string _fileName = "logfile.txt";
        string _adress = "";

    }
        public void write()
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", e.id_);
            }
        }
    
}
