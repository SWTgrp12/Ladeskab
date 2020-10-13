using System;
using System.Collections.Generic;
using System.Text;
using UsbSimulator;

namespace Library
{

    

    public class ChargeControl
    {
        private readonly IUsbCharger _usbChargerSimulator;
        public double current_stat { get; set;}

        public ChargeControl(IUsbCharger usbDevice) 
        { 
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
                // not connected
            }

            else if (((current_stat > 0) && (current_stat <= 5)))
            {
                // charging finished

            }

            else if (((current_stat > 5) && (current_stat <= 500)))
            {
                // charging in progress
            }

            else if (current_stat > 500)
            {
                // charging failure
            }

            else
            {
                // do nothing
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