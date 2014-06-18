using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bettenverwaltung;

namespace BettenverwaltungTests1.Bettenverwaltung
{
    [TestClass]
    public class DBCleanerTests
    {
        [TestMethod]
        public void CleanBedTest()
        {
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
    }
}
