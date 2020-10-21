using Library;
using NUnit.Framework;
using UsbSimulator;
using Display;

namespace Ladeskab.Test
{
    public class DisplayTestClass
    {
        private IDisplay _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new DisplayControl();
        }

        [Test]
        public void PrintStationMsg()
        {
            _uut.PrintStationMsg("Test");
            Assert.That(_uut.DisplayAreaStation, Is.EqualTo());


        }
    }
}