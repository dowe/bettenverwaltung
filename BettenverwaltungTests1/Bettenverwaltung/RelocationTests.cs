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
    public class RelocationTests
    {
        [TestMethod()]
        public void RelocationTest()
        {
            Bed Bed = new Bed();
            Relocation Test = new Relocation(Bed, EStation.Gynaekologie);
            Assert.AreEqual(Bed, Test.sourceBed);
            Assert.AreEqual(3, Test.station);
            Assert.AreEqual(false, Test.accepted);
            Assert.AreEqual(null, Test.timestamp);
            Assert.AreEqual(null, Test.destinationBed);
        }

        [TestMethod()]
        public void GetStationTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Innere_Medizin);
            Assert.AreEqual(EStation.Innere_Medizin, Test.GetStation());
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetActiveTest()
        {
            Bed Bed = new Bed();
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Test.SetActive(Bed);
            Assert.AreEqual(Bed, Test.destinationBed);
            Test.SetActive(new Bed());  //Exception
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetActiveTest_exception1()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Bed Bed = new Bed();
            Bed.cleaningTime = (DateTime.Now.ToString());
            Test.SetActive(Bed);  //Exception
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetActiveTest_exception2()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Bed Bed = new Bed();
            Bed.inRelocation = true;
            Test.SetActive(Bed); //Exception
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetActiveTest_exception3()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Bed Bed = new Bed();
            Bed.patient = new Patient("Peter", "Enis", DateTime.Now, false, EStation.Orthopaedie);
            Test.SetActive(Bed); //Exception
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetInactiveTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Test.SetActive(new Bed());
            Test.SetInactive();
            Assert.AreEqual(null, Test.destinationBed);
            Test.SetInactive(); //Exception
        }

        [TestMethod()]
        public void IsActiveTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Assert.AreEqual(false, Test.IsActive());
            Test.SetActive(new Bed());
            Assert.AreEqual(true, Test.IsActive());
        }

        [TestMethod()]
        public void IsAcceptedTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Assert.AreEqual(false, Test.IsAccepted());
            Test.timestamp = "19.5.2004";
            Assert.AreEqual(true, Test.IsAccepted());
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetAcceptedTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Test.SetAccepted(); //Exception
            Bed Bed = new Bed();
            Test.SetActive(Bed);
            Test.SetAccepted();
            Assert.AreEqual(true, Test.accepted);
            Assert.AreEqual(true, Bed.inRelocation);
            Assert.AreNotEqual(null, Test.timestamp);
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void SetUnacceptedTest()
        {
            Relocation Test = new Relocation(new Bed(), EStation.Gynaekologie);
            Bed Bed = new Bed();
            Test.SetActive(Bed);
            Test.SetAccepted();
            Test.SetUnaccepted();
            Assert.AreEqual(false, Test.accepted);
            Assert.AreEqual(false, Bed.inRelocation);
            Assert.AreEqual(null, Test.timestamp);
            Test.SetUnaccepted(); //Exception
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void ExecuteRelocationTest()
        {
            Bed sourceBed = new Bed();
            Patient Pat = new Patient("Peter", "Enis", DateTime.Now, false, EStation.Orthopaedie);
            sourceBed.patient = Pat;
            Relocation Test = new Relocation(sourceBed, EStation.Gynaekologie);
            Bed destBed = new Bed();
            Test.ExecuteRelocation();
            Test.SetActive(destBed);
            Test.ExecuteRelocation();
            Test.SetAccepted();
            Test.ExecuteRelocation();
            Assert.AreEqual(Pat, destBed.patient);
            Assert.AreEqual(2, Pat.GetHistory().GetSize());
            
        }

     
    }
}
