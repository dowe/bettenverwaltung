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
    public class ControllerTests
    {
        [TestMethod()]
        public void AcceptRelocationTest()
        {
            BVContext db = new BVContext();
            Patient pat = new Patient("Peter", "Enis",new DateTime(),false,1,1,1);
            Bed source = new Bed();
            source.bedId = 1;
            source.station = 1;
            source.patient = pat;
            Relocation Rel = new Relocation(source,EStation.Paediatrie);
            Bed dest = new Bed();
            dest.bedId = 2;
            dest.station = (int)EStation.Paediatrie;
            Rel.SetActive(dest);
            db.Relocations.Add(Rel);
            db.SaveChanges();
            int id = Rel.GetId();
            Controller Cont = new Controller();
            Cont.AcceptRelocation(id);
            db = new BVContext();
            Rel = db.Relocations.Find(id);
            Assert.AreEqual(true, Rel.IsAccepted());
        }

        [TestMethod()]
        public void GetActiveRelocationListTest()
        {
            BVContext db = new BVContext();
            db.Relocations.RemoveRange(db.Relocations.ToArray());
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, 1, 1, 1);
            Bed source = new Bed();
            source.bedId = 1;
            source.station = 1;
            source.patient = pat;
            Relocation Rel = new Relocation(source, EStation.Paediatrie);
            Bed dest = new Bed();
            dest.bedId = 2;
            dest.station = (int)EStation.Paediatrie;
            Rel.SetActive(dest);
            db.Relocations.Add(Rel);
            db.SaveChanges();

            pat = new Patient("Peter", "Enis", new DateTime(), false, 1, 1, 1);
            source = new Bed();
            source.bedId = 1;
            source.station = 1;
            source.patient = pat;
            Rel = new Relocation(source, EStation.Paediatrie);
            dest = new Bed();
            dest.bedId = 2;
            dest.station = (int)EStation.Paediatrie;
            Rel.SetActive(dest);
            db.Relocations.Add(Rel);
            db.SaveChanges();

            pat = new Patient("Peter", "Enis", new DateTime(), false, 1, 1, 1);
            source = new Bed();
            source.bedId = 1;
            source.station = 1;
            source.patient = pat;
            Rel = new Relocation(source, EStation.Paediatrie);
            db.Relocations.Add(Rel);
            db.SaveChanges();

            Controller cont = new Controller();
            List<Relocation> Rellist = cont.GetActiveRelocationList();

            Assert.AreEqual(2, Rellist.Count);
        }
    }
}
