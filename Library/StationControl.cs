﻿using Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbSimulator;
using Display;
//using Door = Door.Door;

namespace Ladeskab
{
   
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        public LadeskabState State;

      //  private IUsbCharger _charger;
        private RFIDReader _rfidReader;
        private ChargeControl _chargeControl;
        private UsbChargerSimulator _usbCharger;
        public IDoor _door;
        private int _oldId;
        private IDisplay _display;
        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        public void SetOldId(int id)
        {
            _oldId = id;
        }

        public int GetOldId()
        {
            return _oldId;
        }
        
        // Normal constructor
        public StationControl()
        {
            // in the constructor we need to create an instance of the class triggering the event.
            _rfidReader =  new RFIDReader();
            // and associate its event handler with the function that handles it.
            _rfidReader.RfidHandler += new EventHandler<RfidEventArgs>(RfidDetected);

            // usbsimulator
            _usbCharger = new UsbChargerSimulator();

            //Display 
            _display = new DisplayControl();
            // Charge Control
            _chargeControl = new ChargeControl(_usbCharger, _display);

            // Door and its events
            _door =  new Door();
            _door.OpenHandler += new EventHandler(DoorOpened);
            _door.CloseHandler += new EventHandler(DoorClosed);

        }
        // Test constructor, which we can easily give substitutes
        public StationControl(RFIDReader rfidReader, UsbChargerSimulator usb, IDoor door)
        {
            // in the constructor we need to create an instance of the class triggering the event.
            _rfidReader = rfidReader;
            // and associate its event handler with the function that handles it.
            _rfidReader.RfidHandler += new EventHandler<RfidEventArgs>(RfidDetected);

            // usbsimulator
            _usbCharger = usb;

            //Display 
            _display = new DisplayControl();
            // Charge Control
            _chargeControl = new ChargeControl(_usbCharger, _display);

            // Door and its events
            _door = door;
            _door.OpenHandler += new EventHandler(DoorOpened);
            _door.CloseHandler += new EventHandler(DoorClosed);
        }
        // event handlers for Door. Displays the appropriate message when the door is opened and closed
        private void DoorOpened(object sender, EventArgs e)
        {
            _display.PrintStationMsg("Tilslut telefon");
        }
        private void DoorClosed(object sender, EventArgs e)
        {
            _display.PrintStationMsg("Indlæs RFID");
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        public void RfidDetected(object sender, RfidEventArgs e)

        {
            Console.WriteLine("RfidDetected called!");
            switch (State)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_chargeControl.connection_establishment())
                    {
                        _door.Lock();
                        _chargeControl.charge_control_start();
                        _oldId = e.id_;
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", e.id_);
                        }
                        _display.PrintStationMsg("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        State = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.PrintStationMsg("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (e.id_ == _oldId)
                    {
                        _chargeControl.charge_control_stop();
                        _door.Unlock();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", e.id_);
                        }

                        _display.PrintStationMsg("Tag din telefon ud af skabet og luk døren");
                        State = LadeskabState.Available;
                    }
                    else
                    {
                        _display.PrintStationMsg("Forkert RFID tag");
                    }

                    break;
                default:
                    // shouldn't happen but might as well be safe.
                    break;
            }
        }
    }
}
