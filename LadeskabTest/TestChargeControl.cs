using Library;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReceivedExtensions;

namespace LadeskabTest
{
    public class Tests
    {
        private IUsbCharger _usbCharger;
        private IDisplay _display;
        private ChargeControl _uut;
        //Charge_Status stat;

        [SetUp]
        public void Setup()
        {
            
            _usbCharger = Substitute.For<IUsbCharger>();
            _display = Substitute.For<IDisplay>();
            _uut = new ChargeControl(_usbCharger,_display);
        }

        [TestCase(false,false)]
        [TestCase(true, true)]
        public void connection_establishment_test(bool Set_con, bool Get_con)
        {
            _usbCharger.Connected.Returns(Set_con);
            Assert.That(_uut.connection_establishment().Equals(Get_con));
        }

        [TestCase(0.0)]
        [TestCase(39.5)]
        [TestCase(500.0)]
        [TestCase(0.1) ]
        public void current_event_test(double current_t)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs {Current = current_t});
            Assert.That(_uut.current_stat.Equals(current_t));
        }

        // using mock to test if call has been issued with correct message dependant on current
        [TestCase(0, "Charger is not connected")]
        [TestCase(5.0, "Charging has finished")]
        [TestCase(3.5, "Charging has finished")]
        [TestCase(0.1, "Charging has finished")]
        [TestCase(5.1, "Charging is currently in progress")]
        [TestCase(350, "Charging is currently in progress")]
        [TestCase(210.55, "Charging is currently in progress")]
        [TestCase(500.0, "Charging is currently in progress")]
        [TestCase(500.1, "Charging failure! Please Disconnect your Device")]
        [TestCase(752.3, "Charging failure! Please Disconnect your Device")]
        [TestCase(1000.0, "Charging failure! Please Disconnect your Device")]
        public void handle_charge_test(double current_t, string msg)
        {
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current_t });
            _display.Received().PrintChargerMsg(msg);
        }

        [Test]
        public void charge_control_start_test()
        {
            _uut.charge_control_start();
            _usbCharger.Received().StartCharge();
        }

        [Test]
        public void charge_control_stop_test()
        {
            _uut.charge_control_stop();
            _usbCharger.Received().StopCharge();
        }
    }
}