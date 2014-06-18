using Bettenverwaltung;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bettenverwaltung.Tests
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
            ControllerTests.ClearDB();

            DateTime dt = new DateTime(2000, 5, 7);
            Patient p1 = new Patient("peter", "enis", dt, true, EStation.Orthopaedie);
            db.Patients.Add(p1);
            db.SaveChanges();
            Patient[] patients = db.Patients.Where(p => p.lastname == "enis").ToArray();

            Assert.AreEqual(patients[0].lastname, "enis");
            foreach (var pat in patients)
            {
                Assert.AreEqual(pat.lastname, "enis");
            }
            db.Patients.Remove(patients[0]);
            db.SaveChanges();
        }

        [TestMethod]
        public void DBBedTest()
        {
            ControllerTests.ClearDB();

            IQueryable<Bed> bedQuery = from b in db.Beds
                                       select b;
            foreach (var bed in bedQuery)
            {
                if ((0 < bed.bedId) && (bed.bedId < 51))
                {
                    Assert.AreEqual(bed.station, (int)EStation.Paediatrie);
                }
                else if ((50 < bed.bedId) && (bed.bedId < 101))
                {
                    Assert.AreEqual(bed.station, (int)EStation.Gynaekologie);
                }
                else if ((100 < bed.bedId) && (bed.bedId < 151))
                {
                    Assert.AreEqual(bed.station, (int)EStation.Innere_Medizin);
                }
                else if ((150 < bed.bedId) && (bed.bedId < 201))
                {
                    Assert.AreEqual(bed.station, (int)EStation.Orthopaedie);
                }
            }
        }
    }
}
