using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    interface IChargeControl
    {
        public bool connection_establishment();
        public Charge_Status handle_charge();
        public void charge_control_start();
        public void charge_control_stop();
    }
}
