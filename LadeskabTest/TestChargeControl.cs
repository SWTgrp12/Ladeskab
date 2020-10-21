using Library;
using NUnit.Framework;
using UsbSimulator;
using NSubstitute;

namespace Ladeskab.Test
{
    public class Tests
    {
        public IUsbCharger _usbCharger;
        public ChargeControl _uut;

        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUsbCharger>(); 
            _uut = new ChargeControl(_usbCharger);
        }

        [Test]
        public void connection_establishment_test()
        {
            Assert.That(_uut.connection_establishment().Equals(true));

        }
    }
}