using Display;
using Library;
using NUnit.Framework;
using UsbSimulator;
using NSubstitute;

namespace LadeskabTest
{
    public class Tests
    {
        private IUsbCharger _usbCharger;
        private IDisplay _display;
        private ChargeControl _uut;
        Charge_Status stat;

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

        [TestCase(0, Charge_Status.NOT_CONNECTED)]
        [TestCase(5.0,Charge_Status.CHARGING_FINISHED)]
        [TestCase(3.5, Charge_Status.CHARGING_FINISHED)]
        [TestCase(0.1, Charge_Status.CHARGING_FINISHED)]
        [TestCase(5.1, Charge_Status.CHARGING_IN_PROGRESS)]
        [TestCase(350, Charge_Status.CHARGING_IN_PROGRESS)]
        [TestCase(210.55, Charge_Status.CHARGING_IN_PROGRESS)]
        [TestCase(500.0, Charge_Status.CHARGING_IN_PROGRESS)]
        [TestCase(500.1, Charge_Status.CHARGING_FAILURE)]
        [TestCase(752.3, Charge_Status.CHARGING_FAILURE)]
        [TestCase(1000.0, Charge_Status.CHARGING_FAILURE)]
        public void handle_charge_test(double current_t, Charge_Status CS_t)
        {
            _uut.current_stat = current_t;
            Assert.That(_uut.handle_charge(), Is.EqualTo(CS_t));
        }

       
    }
}