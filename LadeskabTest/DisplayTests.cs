using Library;
using NUnit.Framework;
using UsbSimulator;
using Display;

namespace LadeskabTest
{
    public class DisplayTestClass
    {
        private IDisplay _uut;
        private string TestMsg;

        [SetUp]
        public void Setup()
        {
            _uut = new DisplayControl();
        }
        /*
        [Test]
        public void PrintStationMsg()
        {
            TestMsg = "Test";
            _uut.PrintStationMsg(TestMsg);
            Assert.That(_uut.DisplayAreaStation, Is.EqualTo(TestMsg));


        }
        */
    }
}