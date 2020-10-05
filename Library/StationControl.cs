using Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using afl2;
using UsbSimulator;

namespace Ladeskab
{
   
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IUsbCharger _charger;
        private RFIDReader _rfidReader;
        private IDoor _door;
        private int _oldId;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        public StationControl()
        {
            // in the constructor we need to create an instance of the class triggering the event.
            _rfidReader =  new RFIDReader();
            // and associate its event handler with the function that handles it.
            _rfidReader.RfidHandler += new EventHandler<RfidEventArgs>(RfidDetected);
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        // object sender burde i teorien betyde at den her 
        private void RfidDetected(object sender, RfidEventArgs e)

        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.Connected)
                    {
                        _door.Lock();
                        _charger.StartCharge();
                        _oldId = e.id_;
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", e.id_);
                        }

                        Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (e.id_ == _oldId)
                    {
                        _charger.StopCharge();
                        _door.Unlock();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", e.id_);
                        }

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
                default:
                    // shouldn't happen but might as well be safe.
                    break;
            }
        }

        // Her mangler de andre trigger handlere
    }
}
