using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bettenverwaltung;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Bettenverwaltung.Tests
{
    [TestClass()]
    public class PatientTests
    {
        [TestMethod()]
        public void PatientTest()
        {
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.firstname, "Peter");
            Assert.AreEqual(pat.lastname, "Mustermann");
            Assert.AreEqual(pat.birthday, new DateTime().ToString());
            Assert.AreEqual(pat.history.historyItem[0].historyItemId, 0);
            Assert.AreEqual(pat.correctStation, (int)EStation.Orthopaedie);
        }

        [TestMethod()]
        public void GetHistoryTest()
        {
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.history, pat.GetHistory());
        }

    
        [TestMethod]
        public void GetFirstNameTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.GetFirstName(), "Erfundener");
        }

        [TestMethod]
        public void GetLastNameTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.GetLastName(), "Name");
        }

        [TestMethod]
        public void GetBirthdayTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(1993, 4, 15, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.IsTrue((new DateTime(1993, 4, 15, 0, 0, 0, 0)).CompareTo(pat.GetBirthday()) == 0);
        }

        [TestMethod]
        public void GetPatientIdTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(1993, 4, 15, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.GetPatientId(), pat.patId);
        }

        [TestMethod]
        public void IsFemaleTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(1993, 4, 15, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.IsFalse(pat.IsFemale());
        }

        [TestMethod]
        public void GetCorrectStationTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(1993, 4, 15, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(pat.GetCorrectStation(), EStation.Orthopaedie);
        }

        [TestMethod()]
        public void GetAgeTest()
        {
            Patient pat = new Patient("Erfundener", "Name", new DateTime(DateTime.Now.Year - 21, DateTime.Now.Month + 1, DateTime.Now.Day, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(20, pat.GetAge());
            pat = new Patient("Erfundener", "Name", new DateTime(DateTime.Now.Year - 21, DateTime.Now.Month, DateTime.Now.Day + 1, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(20, pat.GetAge());
            pat = new Patient("Erfundener", "Name", new DateTime(DateTime.Now.Year - 21, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(21, pat.GetAge());
            pat = new Patient("Erfundener", "Name", new DateTime(DateTime.Now.Year - 21, DateTime.Now.Month -1, DateTime.Now.Day, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(21, pat.GetAge());
            pat = new Patient("Erfundener", "Name", new DateTime(DateTime.Now.Year - 21, DateTime.Now.Month, DateTime.Now.Day - 1, 0, 0, 0, 0), false, EStation.Orthopaedie);
            Assert.AreEqual(21, pat.GetAge());
        }
    }
}


