using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{



    public class ChargeControl: IChargeControl
    {
        private readonly IUsbCharger _usbChargerSimulator;
        private IDisplay _display;
        public double current_stat { get; set;}

        public ChargeControl(IUsbCharger usbDevice, IDisplay display)
        {
            _display = display;
            _usbChargerSimulator = usbDevice;
            _usbChargerSimulator.CurrentValueEvent += UsbDevice_CurrentValueEvent;
        }

        
        private void UsbDevice_CurrentValueEvent(object sender, CurrentEventArgs e) //call this function on event
        {
            current_stat = e.Current;
            handle_charge();
        }

        public bool connection_establishment()
        {
            return _usbChargerSimulator.Connected;
        }

        public void handle_charge()
        {
            // currently you display messages here, but im thinking returning enums to station control that can then decide what to display
            if (current_stat == 0)
            {
                string Msg = "Charger is not connected";
                _display.PrintChargerMsg(Msg);

            }
           
            else if (((current_stat > 0) && (current_stat <= 5)))
            {
                string Msg = "Charging has finished";
                _display.PrintChargerMsg(Msg);

            }

            else if (((current_stat > 5) && (current_stat <= 500)))
            {
                string Msg = "Charging is currently in progress";
                _display.PrintChargerMsg(Msg);
            }

            else if (current_stat > 500)
            {
                string Msg = "Charging failure! Please Disconnect your Device";
                _display.PrintChargerMsg(Msg);
            }
           
            else
            {
            }
        }

        public void charge_control_start()
        {
            _usbChargerSimulator.StartCharge();
        }

        public void charge_control_stop()
        {
            _usbChargerSimulator.StopCharge();
        }

    }
}