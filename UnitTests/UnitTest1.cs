using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bettenverwaltung;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Patient pat = new Patient(null,null,System.DateTime.Now,false,0,0,0);
        }
    }
}
