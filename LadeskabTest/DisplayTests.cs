using Library;
using NUnit.Framework;
using UsbSimulator;

namespace Ladeskab.Test
{
    public class DisplayTestClass
    {
        [SetUp]
        public void Setup()
        {
            var n = new ChargeControl(new UsbChargerSimulator());
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}