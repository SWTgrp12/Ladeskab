using System;
using System.Collections.Generic;
using System.Text;
using Display;
using UsbSimulator;

namespace Library
{

    
    public enum Charge_Status
    {
        NOT_CONNECTED=1,
        CHARGING_FINISHED=2,
        CHARGING_IN_PROGRESS=3,
        CHARGING_FAILURE=4,
        NON_VALID=5,
    }
    

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
        }

        public bool connection_establishment()
        {
            return _usbChargerSimulator.Connected;
        }

        public Charge_Status handle_charge()
        {
            // currently you display messages here, but im thinking returning enums to station control that can then decide what to display
            if (current_stat == 0)
            {
                string Msg = "Charger is not connected";
                _display.PrintChargerMsg(Msg);
                return Charge_Status.NOT_CONNECTED;
                //return 1;
                // not connected
            }
           
            else if (((current_stat > 0) && (current_stat <= 5)))
            {
                string Msg = "Charging has finished";
                _display.PrintChargerMsg(Msg);
                return Charge_Status.CHARGING_FINISHED;
                //return 2;
                //charging finished

            }

            else if (((current_stat > 5) && (current_stat <= 500)))
            {
                string Msg = "Charging is currently in progress";
                _display.PrintChargerMsg(Msg);
                return Charge_Status.CHARGING_IN_PROGRESS;
                //return 3;
                // charging in progress
            }

            else if (current_stat > 500)
            {
                string Msg = "Charging failure! Please Disconnect your Device";
                _display.PrintChargerMsg(Msg);
                return Charge_Status.CHARGING_FAILURE;
                //return 4;
                // charging failure
            }
           
            else
            {
                return Charge_Status.NON_VALID;
                //return 5;
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