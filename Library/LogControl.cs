using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class LogControl
    {

        public string _adress { get; private set; } = "";
        public string _fileName { get; private set; } = "logfile.txt";

        public LogControl()
        {

        }
        public LogControl(string adress, string filename)
        {
            _adress = adress;
            _fileName = filename;
        }

        public void WriteEntry(string entry)
        {
            String logFile = _adress + _fileName; // Changeable at runtime
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine(DateTime.Now + ": " + entry);
            } // Should DateTime have it's own layer aswell for testing? or not because it is a standard function?
        }
    }
}
