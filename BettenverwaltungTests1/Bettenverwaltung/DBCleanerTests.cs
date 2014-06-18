using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bettenverwaltung;

namespace Bettenverwaltung.Tests
{
    [TestClass]
    public class DBCleanerTests
    {
        [TestMethod]
        public void CleanBedTest()
        {
            ControllerTests.ClearDB();

            BVContext db = new BVContext();
            Bed b1 = db.Beds.Find(1);
            DateTime now = DateTime.Now;
            b1.cleaningTime = (now - new TimeSpan(3, 0, 0)).ToString();
            Bed b2 = db.Beds.Find(2);
            b2.cleaningTime = now.ToString();
            db.SaveChanges();

            DBCleaner cleaner = new DBCleaner(1);
            cleaner.Start();
            System.Threading.Thread.Sleep(5000);
            cleaner.Stop();

            db = new BVContext();
            Bed b1a = db.Beds.Find(1);
            Assert.IsNull(b1a.cleaningTime);
            Bed b2a = db.Beds.Find(2);
            Assert.AreEqual(b2a.cleaningTime, now.ToString());
            b2a.cleaningTime = null;
            db.SaveChanges();
        }

        [TestMethod]
        public void SetRelocationActiveTest()
        {
            ControllerTests.ClearDB();

            BVContext db = new BVContext();
            Patient p = new Patient("klaus", "Müller", DateTime.Now, false, EStation.Orthopaedie);
            db.Patients.Add(p);
            Bed b_gyn = db.Beds.Find(1);
            b_gyn.SetPatient(p);
            DateTime now = DateTime.Now;
            Bed b_orth = db.Beds.Find(151);
            b_orth.cleaningTime = (now - new TimeSpan(3,0,0)).ToString();
            Relocation rel1 = new Relocation(b_gyn, EStation.Orthopaedie);
            db.Relocations.Add(rel1);
            db.SaveChanges();

            DBCleaner cleaner = new DBCleaner(1);
            cleaner.Start();
            System.Threading.Thread.Sleep(5000);
            cleaner.Stop();

            db = new BVContext();
            Relocation rel2 = db.Relocations.Find(rel1.relocationId);
            Assert.AreEqual(rel2.destinationBed.bedId,b_orth.bedId);

        }

        [TestMethod]
        public void RemoveForgottenAcceptedRelocationsTest()
        {
            ControllerTests.ClearDB();

            BVContext db = new BVContext();
            Bed b = db.Beds.Find(1);
            Relocation r = new Relocation(b, EStation.Innere_Medizin);
            r.destinationBed = db.Beds.Find(2);
            DateTime now = DateTime.Now;
            r.timestamp = (now -  new TimeSpan(1, 0, 0)).ToString();
            r.accepted = true;
            db.Relocations.Add(r);
            db.SaveChanges();

            DBCleaner cleaner = new DBCleaner(1);
            cleaner.Start();
            System.Threading.Thread.Sleep(5000);
            cleaner.Stop();

            db = new BVContext();
            Relocation r2 = db.Relocations.Find(r.relocationId);
            Assert.IsNull(r2.timestamp);
            Assert.AreEqual(r2.destinationBed.bedId, 2);

            ControllerTests.ClearDB();
        }
    }
}
