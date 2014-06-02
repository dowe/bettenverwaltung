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
            HistoryItem item = HistoryItem.CreateRelocationItem(1, 2, 3);
            Assert.IsNotNull(item.text);
        }

        [TestMethod()]
        public void CreateEntryItemTest()
        {
            HistoryItem item = HistoryItem.CreateEntryItem(0);
            Assert.IsNotNull(item.text);
        }
    }
}

