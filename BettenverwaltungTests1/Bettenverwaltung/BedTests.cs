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
    public class BedTests
    {
        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetPatientTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.patient = null;
            bed.cleaningTime = null;

            bed.SetPatient(pat);
            Assert.AreEqual(pat, bed.patient);

            //for exception
            bed.SetPatient(pat);
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void RemovePatientTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            bed.RemovePatient();
            Assert.IsNull(bed.patient);

            //for exception
            bed.RemovePatient();
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetInRelocationTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            bed.SetInRelocation(false);
            Assert.IsFalse(bed.inRelocation);

            bed.SetInRelocation(true);
            //for exception
            bed.SetInRelocation(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void StartCleaningTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            bed.StartCleaning();
            Assert.IsNotNull(bed.cleaningTime);

            //for exception
            bed.StartCleaning();
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void StopCleaningTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = (DateTime.Now.ToString());

            bed.StopCleaning();
            Assert.IsNull(bed.cleaningTime);

            //for exception
            bed.StopCleaning();
        }

        [TestMethod()]
        public void GetCleaningTimeTest()
        {
            Bed bed = new Bed();
            //initialize bed
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = new DateTime().ToString();

            Assert.AreEqual(bed.GetCleaningTime(), DateTime.Parse(bed.cleaningTime));
        }

        [TestMethod()]
        public void GetPatientTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.AreEqual(bed.GetPatient(), pat);
        }

        [TestMethod()]
        public void GetStationTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.AreEqual((int)bed.GetStation(), 0);
        }

        [TestMethod()]
        public void GetBedIdTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.AreEqual(bed.GetBedId(), 0);
        }

        [TestMethod()]
        public void IsEmptyTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.IsFalse(bed.IsEmpty());
            bed.patient = null;
            Assert.IsTrue(bed.IsEmpty());
        }

        [TestMethod()]
        public void IsGettingCleanedTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.IsFalse(bed.IsGettingCleaned());
            bed.cleaningTime = (DateTime.Now.ToString());
            Assert.IsTrue(bed.IsGettingCleaned());
        }

        [TestMethod()]
        public void IsInRelocationTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            bed.patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.IsTrue(bed.IsInRelocation());
            bed.inRelocation = false;
            Assert.IsFalse(bed.IsInRelocation());
        }
    }
}