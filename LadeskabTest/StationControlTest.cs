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
        private IRFIDReader _rfidreader;
        private IDoor _door;
        private IUsbCharger _usb;
        private IDisplay _disp;
        private IChargeControl _chargeControl;
        private StationControl _uut;

        // Strings for display 
        string _TestMsg = "Msg";
        string _DoorOpenMsg = "Tilslut telefon";
        string _DoorOpenMsgRfid = "Door is Open";
        string _DoorClosedMsg = "Indlæs RFID";
        //string _RfidMsg = "Indlæs RFID";
        string _NoConnectionMsg = "Din telefon er ikke ordentlig tilsluttet.Prøv igen.";
        string _NowChargingMsg = "Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.";
        string _TakePhoneMsg = "Tag din telefon ud af skabet og luk døren";
        // Vi Checker hvilken State vi er i ved
        // at checke beskederne der bliver sendt til Display

        [SetUp]
        public void Setup()
        {
            _rfidreader = Substitute.For<IRFIDReader>();
            _door = Substitute.For<IDoor>();
            _usb = Substitute.For<IUsbCharger>();
            _disp = Substitute.For<IDisplay>();
            _chargeControl = Substitute.For<IChargeControl>();
            _uut = new StationControl(_rfidreader, _door, _disp, _chargeControl);
        }

        [Test]
        public void Door_Opened()
        {
            // This also tests that Stationcontrol starts in the available state
            _door.OpenHandler += Raise.Event();
            _disp.Received().PrintStationMsg(_DoorOpenMsg);
        }
        [Test]
        public void Door_Closed()
        {
            _door.OpenHandler += Raise.Event();
            _disp.Received().PrintStationMsg(_DoorOpenMsg);
            _door.CloseHandler += Raise.Event();
            _disp.Received().PrintStationMsg(_DoorClosedMsg);
        }

        [Test]
        public void Door_Closed_Fail()
        {
            // Door is allready closed
            _door.CloseHandler += Raise.Event();
            _disp.DidNotReceiveWithAnyArgs().PrintStationMsg(_TestMsg); 
            // DidNotRecieve should check for any call, but we can't call without a dummy Msg for PrintStationMsg 
        }

        [TestCase(1)]
        [TestCase(9)]
        public void Rfid_AvailableState(int id)
        {
            _chargeControl.connection_establishment().Returns(true); // Ensure phone connection get's returned True 
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            _door.Received().Lock();
            _chargeControl.Received().charge_control_start();
            _disp.Received().PrintStationMsg(_NowChargingMsg);
        }
        public void Rfid_AvailableState_NoConnection(int id)
        {
            _chargeControl.connection_establishment().Returns(false);  
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            _door.DidNotReceive().Lock();
            _chargeControl.DidNotReceive().charge_control_start();
            _disp.Received().PrintStationMsg(_NoConnectionMsg);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Rfid_OpenState(int id)
        {
            _door.OpenHandler += Raise.Event();
            _disp.Received().PrintStationMsg("Tilslut telefon");
            // State is now DoorOpen
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            _door.DidNotReceive().Lock();
            _chargeControl.DidNotReceive().charge_control_start();
            _disp.Received().PrintStationMsg(_DoorOpenMsgRfid);
        }

        [TestCase(30)]
        [TestCase(900)]
        public void Rfid_LockedState(int id)
        {
            // Get Locked state with ID
            _chargeControl.connection_establishment().Returns(true); // 
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            _door.Received().Lock();
            _chargeControl.Received().charge_control_start();
            // State now Locked. open it again with the same ID
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            //Assert.AreEqual(id, station._oldId);
            _door.Received().Unlock();
            _disp.PrintStationMsg("Tag din telefon ud af skabet og luk døren");
        }
        // TODO: Needs specific test cases
        [TestCase(1, 1)]
        [TestCase(1000, 1000)]
        public void TestRfidOpenNoConnection(int OldId,int id)
        {
            // set old_id
            station.State = StationControl.LadeskabState.Available;
            chargeControl.connection_establishment().Returns(false); 
            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(OldId));

            rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));

            // assert if old_id should match or not
            Assert.AreNotEqual(station._oldId, id);
            Assert.AreEqual(StationControl.LadeskabState.Available, station.State);
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