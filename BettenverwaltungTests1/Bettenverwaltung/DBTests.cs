using Bettenverwaltung;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BettenverwaltungTests1.Bettenverwaltung
{
    [TestClass]
    public class DBTests
    {
        static BVContext db;
        [ClassInitialize]
        public static void GetDBContext(TestContext context)
        {
            db = new BVContext();
        }

        [TestMethod]
        public void DBPatientTest()
        {
            DateTime dt = new DateTime(2000,5,7);
            Patient p1 = new Patient("peter","enis",dt,true,25,0,0);
            db.Patients.Add(p1);
            db.SaveChanges();
            Patient patients = db.Patients.Find(25);
            Assert.AreEqual(patients.patId,25);
        }

        [TestMethod]
        public void DBBedTest()
        {
            Bed b0 = new Bed();
            b0.bedId = 0;
            db.Beds.Add(b0);
            Bed b1 = new Bed();
            b1.bedId = 200;
            db.Beds.Add(b1);

            db.SaveChanges();

            var beds = from b in db.Beds orderby b.bedId select b;
            foreach (var bed in beds)
            {
                Console.WriteLine(bed.bedId.ToString());
            }
        }
    }
}
