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
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            Assert.AreEqual(pat.firstname, "Peter");
            Assert.AreEqual(pat.lastname, "Mustermann");
            Assert.AreEqual(pat.birthday, new DateTime().ToString());
            Assert.AreEqual(pat.history.historyItem[0].historyItemId, 0);
        }

        [TestMethod()]
        public void GetHistoryTest()
        {
            Patient pat = new Patient("Peter", "Mustermann", new DateTime(), false);
            Assert.AreEqual(pat.history, pat.GetHistory());
        }
    }
}


