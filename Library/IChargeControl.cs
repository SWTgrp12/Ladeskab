using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public interface IChargeControl
    {
        public bool connection_establishment();
        public void handle_charge();
        public void charge_control_start();
        public void charge_control_stop();
    }
}
