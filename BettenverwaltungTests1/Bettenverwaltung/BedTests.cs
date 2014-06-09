﻿using System;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.Patient = null;
            bed.cleaningTime = null;

            bed.SetPatient(pat);
            Assert.AreEqual(pat, bed.Patient);

            //for exception
            bed.SetPatient(pat);
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void RemovePatientTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            bed.RemovePatient();
            Assert.IsNull(bed.Patient);

            //for exception
            bed.RemovePatient();
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetInRelocationTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = DateTime.Now;

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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = new DateTime();

            Assert.AreEqual(bed.GetCleaningTime(), bed.cleaningTime);
        }

        [TestMethod()]
        public void GetPatientTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.IsFalse(bed.IsEmpty());
            bed.Patient = null;
            Assert.IsTrue(bed.IsEmpty());
        }

        [TestMethod()]
        public void IsGettingCleanedTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            Assert.IsFalse(bed.IsGettingCleaned());
            bed.cleaningTime = DateTime.Now;
            Assert.IsTrue(bed.IsGettingCleaned());
        }

        [TestMethod()]
        public void IsInRelocationTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
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