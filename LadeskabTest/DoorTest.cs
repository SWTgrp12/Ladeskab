using System;
using System.Collections.Generic;
using System.Text;
using Library;
using NUnit.Framework;
using NSubstitute;

namespace LadeskabTest
{
    class DoorTest
    {
         private IDoor door;
         //private IDoor door = Substitute.For<IDoor>();
         [SetUp]
         public void Setup()
         {
             door = new Library.Door();
         }
        
         [Test]
         public void TestDoorStartsClosed()
         {
             Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Closed));


        }
        [Test]
        public void TestDoorOpen()
        {
            //door._state = Door.DoorState.Closed;
            door.Open();
            Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Open));
            // check the event maybe?
        }

        [Test]
        public void TestDoorOpenWhenLocked()
        {
            door.Lock();
            door.Open();
            Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Locked));

        }
        [Test]
        public void TestDoorClose()
        {
            // opening the door so we know the initial state
            door.Open();
            door.Close();
            Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Closed));
            // check the event maybe?
        }
        [Test]
        public void TestDoorLock()
        {
            door.Lock();
            Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Locked));
        }
        [Test]
        public void TestDoorUnlock()
        {
            door.Lock();
            door.Unlock();
            Assert.That(door.GetDoorState(), Is.EqualTo(Door.DoorState.Closed));
        }
        [Test]
        public void TestDoorOpenEventRaised()
        {
            var wasCalled = false;
            door.OpenHandler += (sender, args) => wasCalled = true;
            //Tell the substitute to raise the event with a sender and EventArgs:
            //door.OpenHandler += Raise.EventWith(new object(), new EventArgs());
            door.Open();
            Assert.True(wasCalled);

        }
        [Test]
        public void TestDoorCloseEventRaised()
        {
            door.Open();
            var wasCalled = false;
            door.CloseHandler += (sender, args) => wasCalled = true;
            //Tell the substitute to raise the event with a sender and EventArgs:
            //door.OpenHandler += Raise.EventWith(new object(), new EventArgs());
            door.Close();
            Assert.True(wasCalled);

        }
    }
}
