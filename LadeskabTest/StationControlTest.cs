using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Display;
using Ladeskab;
using Library;
using UsbSimulator;
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

        [TestCase(1)]
        [TestCase(9)]
        public void TestRfidAvailable(int id)
        {
            station.State = StationControl.LadeskabState.Available;
            chargeControl.connection_establishment().Returns(true); // ENSURE CONNECTION ESTABLISHMENT
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            Assert.AreEqual(id, station.GetOldId());
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
        public void TestRfidLocked(int OldId, int id)
        {
            // set old_id
            station.SetOldId(OldId);

            station.State = StationControl.LadeskabState.Locked;
            // ENSURE CONNECTION ESTABLISHMENT SOMEHOW
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            // assert if old_id should match or not
            Assert.AreEqual(station.GetOldId(), id);
            Assert.AreEqual(StationControl.LadeskabState.Available, station.State);
            Assert.AreEqual(Door.DoorState.Closed, station._door.GetDoorState());
        }
        [TestCase(5,79)]
        [TestCase(58432,28923)]
        public void TestRfidLockedIdMismatch(int OldId, int id)
        {
            // set old_id
            station.SetOldId(OldId);
            station.State = StationControl.LadeskabState.Available;
            // ENSURE CONNECTION ESTABLISHMENT SOMEHOW
            Door.DoorState old_door_state = station._door.GetDoorState();
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            Assert.AreNotEqual(id, station.GetOldId());
            Assert.AreEqual(StationControl.LadeskabState.Locked, station.State);
            // assert door is unchanged somehow??
            Assert.AreEqual(old_door_state, station._door.GetDoorState());

        }
    }
}