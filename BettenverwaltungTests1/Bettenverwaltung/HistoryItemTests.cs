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
    public class HistoryItemTests
    {
        [TestMethod()]
        public void CreateRelocationItemTest()
        {
            Assert.Fail();
        }
    }
}

/*
[TestMethod]
        public void patientTest()
        {
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, 0, 0, 0);
            Assert.AreEqual(pat.firstname, "Peter");
            Assert.AreEqual(pat.lastname, "Enis");
            Assert.AreEqual(pat.birthday, new DateTime());
            Assert.AreEqual(pat.History.HistoryItem[0].historyItemId, 0);
        }

        [TestMethod]
        public void bedTest()
        {
            Bed bed = new Bed();
            //initialize bed
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, 0, 0, 0);
            bed.Patient = pat;
            bed.inRelocation = true;
            bed.station = 0;
            bed.bedId = 0;
            bed.cleaningTime = null;

            //isGettingCleaned()
            Assert.IsFalse(bed.IsGettingCleaned(), "Bed getting cleaned?", null);

            //startCleaning
            bed.StartCleaning();
            Assert.AreNotEqual(bed.cleaningTime, null);

            //stopCleaning
            bed.StopCleaning();
            Assert.AreEqual(bed.cleaningTime, null);

            //SetInRelocation
            bed.SetInRelocation(false);
            Assert.IsFalse(bed.inRelocation);

            //IsInRelocation
            Assert.IsFalse(bed.IsInRelocation());

            //Get/Set/Remove Patient
            Assert.AreEqual(bed.GetPatient(), pat);
            pat = bed.RemovePatient();
            Assert.IsNull(bed.GetPatient());
            bed.SetPatient(pat);
            Assert.AreEqual(pat, bed.Patient);

            //Exceptions
            Assert.AreEqual(bed.RemovePatient(), pat);

            //IsEmpty
            Assert.IsTrue(bed.IsEmpty());
        }

        [TestMethod]
        public void historyTest()
        {
            History hist = new History(0, 0);

            //GetSize
            Assert.AreEqual(hist.GetSize(), 1);

            //GetHistoryItem
            Assert.AreEqual(hist.HistoryItem[0].historyItemId, 0);

            //InserHistoryItem
            HistoryItem item = HistoryItem.CreateRelocationItem(1,2,3);
            hist.InsertHistoryItem(item);
            Assert.AreEqual(hist.HistoryItem[1].historyItemId, 1);

            //GetSize
            Assert.AreEqual(hist.GetSize(), 2);
        }

        [TestMethod]
        public void historyItemTest()
        {
            //CreateEntryItem
            HistoryItem item = HistoryItem.CreateEntryItem(0);
            Assert.IsNotNull(item.text);

            //CreateRelocationItem
            item = HistoryItem.CreateRelocationItem(1, 2, 3);
            Assert.IsNotNull(item.text);
        }
*/