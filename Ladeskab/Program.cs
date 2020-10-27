using System;
using System.Collections.Generic;
using System.Text;
using Library;

namespace LadeskabProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Assemble your system here from all the classes
            Door door = new Door();
            DisplayControl display = new DisplayControl();
            UsbChargerSimulator usbCharger = new UsbChargerSimulator();
            RFIDReader rfidReader = new RFIDReader();

            ChargeControl chargeControl = new ChargeControl(usbCharger, display);
            StationControl stationControl = new StationControl(rfidReader, door, display, chargeControl);



            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast (E)xit, (O)pendoor, (C)losedoor, (R)fid: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                
                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        door.Open();
                        break;

                    case 'C':
                        door.Close();
                        break;

                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        rfidReader.RfidEvent(id);
                        break;

                    default:
                        break;
                }
                
            } while (!finish);

        }

    }
}
