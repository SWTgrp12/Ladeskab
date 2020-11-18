using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Library;
using NUnit.Framework;
using NSubstitute;

namespace LadeskabTest
{
    public class StationControlTest
    {
        private IRFIDReader _rfidreader;
        private IDoor _door;
        private IUsbCharger _usb;
        private IDisplay _disp;
        private IChargeControl _chargeControl;
        private StationControl _uut;

        // Strings for display 
        // Not a great way to check string parameter's, but better than in the test's themself
        string _TestMsg = "Msg";
        string _DoorOpenMsg = "Tilslut telefon";
        string _DoorOpenMsgRfid = "Door is Open";
        string _DoorClosedMsg = "Indlæs RFID";
        string _NoConnectionMsg = "Din telefon er ikke ordentlig tilsluttet. Prøv igen.";
        string _WrongId = "Forkert RFID tag";
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
            RfidEventArgs RfidArgs = new RfidEventArgs(id);
            _chargeControl.connection_establishment().Returns(true);
            _rfidreader.RfidHandler += Raise.EventWith(this ,RfidArgs);
            //_rfidreader.RfidHandler += (sender, args) => receivedId = args.id_;

            _door.Received().Lock();
            _chargeControl.Received().charge_control_start();
            _disp.Received().PrintStationMsg(_NowChargingMsg);
        }
        [TestCase(1)]
        [TestCase(9)]
        public void Rfid_AvailableState_NoConnection(int id)
        {
            RfidEventArgs RfidArgs = new RfidEventArgs(id);
            _chargeControl.connection_establishment().Returns(false);  
            _rfidreader.RfidHandler += Raise.EventWith(RfidArgs);

            _door.DidNotReceive().Lock();
            _chargeControl.DidNotReceive().charge_control_start();
            _disp.Received().PrintStationMsg(_NoConnectionMsg);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Rfid_OpenState(int id)
        {
            _door.OpenHandler += Raise.Event();
            _disp.Received().PrintStationMsg(_DoorOpenMsg);
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
            _door.Received().Unlock();
            _disp.PrintStationMsg(_TakePhoneMsg);
        }

        [TestCase(1, 1)]
        [TestCase(1000, 1000)]
        public void Rfid_OpenNoConnection(int OldId, int id)
        {
            RfidEventArgs RfidArgs = new RfidEventArgs(OldId);
            _chargeControl.connection_establishment().Returns(false); 
            _rfidreader.RfidHandler += Raise.EventWith(RfidArgs);
            _door.DidNotReceive().Lock();
        }
 
        [TestCase(5,79)]
        [TestCase(58432,28923)]
        public void TestRfidLockedIdMismatch(int OldId, int id)
        {
            Assert.AreNotEqual(OldId, id); //tested id's can't match
            // Get Locked state with ID
            _chargeControl.connection_establishment().Returns(true); // 
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(OldId));
            _door.Received().Lock();
            _chargeControl.Received().charge_control_start();
            // State now Locked. open it again with the wrong id
            _rfidreader.RfidHandler += Raise.EventWith(new RfidEventArgs(id));
            // But Id's that are not similar, isn't able to unlock.
            _disp.PrintStationMsg(_WrongId);
        }
    }
}