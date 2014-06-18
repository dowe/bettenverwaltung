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
    public class HistoryTests
    {

        [TestMethod()]
        public void HistoryTest()
        {
            //initialize a History
            History hist = History.CreateNewHistory();

            Assert.AreEqual(hist.historyItem[0].historyItemId, 0);

        }


        [TestMethod()]
        public void GetHistoryItemTest()
        {
            //initialize a History
            History hist = History.CreateNewHistory();

            HistoryItem item = hist.GetHistoryItem(0);
            Assert.AreEqual(item.historyItemId, 0);
        }
       

        [TestMethod()]
        public void InsertHistoryItemTest()
        {
            //initialize a History
            History hist = History.CreateNewHistory();

            HistoryItem item = HistoryItem.CreateRelocationItem(2, 3);
            hist.InsertHistoryItem(item);
            Assert.IsNotNull(hist.historyItem[1].text);

        }

        [TestMethod()]
        public void GetSizeTest()
        {
            //initialize a History
            History hist = History.CreateNewHistory();

            Assert.AreEqual(hist.GetSize(), 1);
            HistoryItem item = HistoryItem.CreateRelocationItem(2, 3);
            hist.InsertHistoryItem(item);
            Assert.AreEqual(hist.GetSize(), 2);
        }
    }
}


