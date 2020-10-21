using System;
using System.Collections.Generic;
using System.Text;
using Library;
using NUnit.Framework;
using NSubstitute;
namespace LadeskabTest
{
    class RFIDTest
    {
        private IRFIDReader rfid_reader; 
        [SetUp]
        public void Setup()
        {
            //door = new Library.Door();
            rfid_reader = new Library.RFIDReader();
        }
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3900)]
        public void TestRFIDEventWithID(int expectedId)
        {
            //var wasCalled = false;
            int receivedId = 0;
            //rfid_reader.RfidHandler += (sender, args) => wasCalled = true;
            rfid_reader.RfidHandler += (sender, args) => receivedId = args.id_;
            //Tell the substitute to raise the event with a sender and EventArgs:
            //door.OpenHandler += Raise.EventWith(new object(), new EventArgs());
            rfid_reader.RfidEvent(expectedId);
            Assert.AreEqual(expectedId, receivedId);
            //Assert.True(wasCalled);

        }

    }
}
