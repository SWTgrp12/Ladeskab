using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Library;
using NUnit.Framework;

namespace LadeskabTest
{
    public class TestLogControl
    {
        private LogControl _uut;
        string _testfileName = "testlogfile.txt";
        string _testadress = "";
        string _testMsg = "Skab låst med RFID: 5678";

    [SetUp]
        public void Setup()
        {
            _uut = new Library.LogControl(_testadress, _testfileName);
            string _file = _testadress + _testfileName;
            if (File.Exists(_file)) // should check note file doesn't allready exist
            {
                File.Delete(_file);
            }
        }

        [Test]
        public void establish_note() //check that notefile is created
        {
            string _file = _testadress + _testfileName;
            Assert.IsFalse(File.Exists(_file));
            _uut.WriteEntry(_testMsg);
            Assert.IsTrue(File.Exists(_file)); 
        }
        [Test]
        public void write_note()
        {
            string _file = _testadress + _testfileName;
            _uut.WriteEntry(_testMsg);
            string _note = File.ReadAllText(_file);
            _note = _note.Substring(21, 24); // removing the date/time stamp, and linechange \r\n
            Assert.IsNotNull(_note);
            Assert.AreEqual(_note, _testMsg); 
        }
    }
}
