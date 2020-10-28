using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Library;
using NUnit.Framework;
using NSubstitute;

namespace LadeskabTest
{
    class StationControlTest
    {
        private IRFIDReader rfidreader;
        private IUsbCharger usb;
        private IDoor door;
        private StationControl station;
        private IDisplay disp;
        private IChargeControl chargeControl;

    [SetUp]
        public void Setup()
        {
            rfidreader = Substitute.For<IRFIDReader>();
            door = Substitute.For<IDoor>();
            usb = Substitute.For<IUsbCharger>();
            disp = Substitute.For<IDisplay>();
            chargeControl = Substitute.For<IChargeControl>();
            station = new StationControl(rfidreader, door, disp, chargeControl);
        }

        [TestCase(30)]
        [TestCase(900)]
        public void TestSetOldId(int id)
        {
            // set old_id
            station.State = StationControl.LadeskabState.Available;
            chargeControl.connection_establishment().Returns(true); // ENSURE CONNECTION ESTABLISHMENT
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            Assert.AreEqual(id, station._oldId);
        }
        [Test]
        public void TestDoorOpened()
        {
            door.OpenHandler += Raise.Event();
            Assert.AreEqual(StationControl.LadeskabState.DoorOpen, station.State);
        }
        [Test]
        public void TestDoorClosed()
        {
            door.OpenHandler += Raise.Event();
            door.CloseHandler += Raise.Event();
            Assert.AreEqual(StationControl.LadeskabState.Available, station.State);

        }
        [Test]
        public void TestDoorClosedFail()
        {
            // Door is allready closed
            door.CloseHandler += Raise.Event();
            Assert.AreEqual(StationControl.LadeskabState.Available, station.State);

        }
        [TestCase(1)]
        [TestCase(9)]
        public void TestRfidAvailable(int id)
        {
            station.State = StationControl.LadeskabState.Available;
            chargeControl.connection_establishment().Returns(true); // ENSURE CONNECTION ESTABLISHMENT
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            Assert.AreEqual(StationControl.LadeskabState.Locked, station.State);
        }
        [TestCase(1)]
        [TestCase(9)]
        public void TestRfidOpen(int id)
        {
            station.State = StationControl.LadeskabState.DoorOpen;
            // ENSURE CONNECTION ESTABLISHMENT SOMEHOW
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            Assert.AreEqual(StationControl.LadeskabState.DoorOpen, station.State);
            // TODO: Does this need other asserts?
            // assert nothing's changed
        }
        // TODO: Needs specific test cases
        [TestCase(1,1)]
        [TestCase(1000,1000)]
        public void TestRfidOpenNoConnection(int id)
        {
            station.State = StationControl.LadeskabState.DoorOpen;
            chargeControl.connection_establishment().Returns(false);
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            Assert.AreEqual(StationControl.LadeskabState.DoorOpen, station.State);
        }
        // TODO: Needs specific test cases
        [TestCase(1, 1)]
        [TestCase(1000, 1000)]
        public void TestRfidLocked(int OldId, int id)
        {
            // set old_id
            station.State = StationControl.LadeskabState.Available;
            chargeControl.connection_establishment().Returns(true); // ENSURE CONNECTION ESTABLISHMENT
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(OldId));

            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            // assert if old_id should match or not
            Assert.AreEqual(station._oldId, id);
            Assert.AreEqual(StationControl.LadeskabState.Available, station.State);
        }
        [TestCase(5,79)]
        [TestCase(58432,28923)]
        public void TestRfidLockedIdMismatch(int OldId, int id)
        {
            // set old_id
            station.State = StationControl.LadeskabState.Available;
            // ENSURE CONNECTION ESTABLISHMENT SOMEHOW
            chargeControl.connection_establishment().Returns(true); // ENSURE CONNECTION ESTABLISHMENT
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(OldId));
            
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            Assert.AreNotEqual(id, station._oldId);
            Assert.AreEqual(StationControl.LadeskabState.Locked, station.State);
        }
    }
}