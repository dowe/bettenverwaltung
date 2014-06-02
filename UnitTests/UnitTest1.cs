using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bettenverwaltung;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void patientTest()
        {
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, 0, 0, 0);
            Assert.Equals(pat.firstname, "Peter");
            Assert.Equals(pat.lastname, "Enis");
            Assert.Equals(pat.birthday, new DateTime());
            Assert.Equals(pat.History.HistoryItem[0].historyItemId, 0);
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
            bed.cleaningTime = new DateTime();

            //isGettingCleaned()
            Assert.IsFalse(bed.IsGettingCleaned(), "Bed getting cleaned?", null);

            //startCleaning
            bed.StartCleaning();
            Assert.AreNotEqual(bed.cleaningTime, new DateTime());

            //stopCleaning
            bed.StartCleaning();
            Assert.Equals(bed.cleaningTime, new DateTime());

            //SetInRelocation
            bed.SetInRelocation(false);
            Assert.IsFalse(bed.inRelocation);

            //IsInRelocation
            Assert.IsFalse(bed.IsInRelocation());

            //Get/Set/Remove Patient
            Assert.Equals(bed.GetPatient(), pat);
            pat = bed.RemovePatient();
            Assert.IsNull(bed.GetPatient());
            bed.SetPatient(pat);
            Assert.Equals(pat, bed.Patient);

            //Exceptions
            bed.RemovePatient();
            Assert.IsNull(bed.RemovePatient());

            //IsEmpty
            Assert.IsTrue(bed.IsEmpty());
        }

        [TestMethod]
        public void historyTest()
        {
            History hist = new History(0, 0);

            //GetSize
            Assert.Equals(hist.GetSize(), 1);

            //GetHistoryItem
            Assert.Equals(hist.HistoryItem[0].historyItemId, 0);

            //InserHistoryItem
            HistoryItem item = HistoryItem.CreateEntryItem(1);
            hist.InsertHistoryItem(item);
            Assert.Equals(hist.HistoryItem[1].historyItemId, 1);

            //GetSize
            Assert.Equals(hist.GetSize(), 2);
        }

        [TestMethod]
        public void historyTest()
        {
            //CreateEntryItem
            HistoryItem item = HistoryItem.CreateEntryItem(0);
            Assert.IsNotNull(item.text);

            //CreateRelocationItem
            item = HistoryItem.CreateRelocationItem(1, 2, 3);
            Assert.IsNotNull(item.text);
        }
    }
}
