﻿using System;
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
                    case 'e':
                    case 'E':
                        finish = true;
                        break;

                    case 'o':
                    case 'O':
                        door.Open();
                        break;

                    case 'c':
                    case 'C':
                        door.Close();
                        break;

                    case 'r':
                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();
                        int id = 0;
                        while (!Int32.TryParse(idString, out id))
                        {
                            System.Console.WriteLine("Rfid not accepted, Try again: ");
                            idString = System.Console.ReadLine();
                        }
                        rfidReader.RfidEvent(id);
                        break;

                    default:
                        break;
                }
                
            } while (!finish);

        }

    }
}
