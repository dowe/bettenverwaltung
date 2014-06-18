﻿using System;
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
        public void ClearDB()
        {
            BVContext db = new BVContext();
            db.Relocations.RemoveRange(db.Relocations.ToArray());
            db.Patients.RemoveRange(db.Patients.ToArray());
            db.Histories.RemoveRange(db.Histories.ToArray());
            db.HistoryItems.RemoveRange(db.HistoryItems.ToArray());

            Bed bed = null;
            for (int i = 1; i <= db.Beds.ToList().Count; i++)
            {
                bed = db.Beds.Find(i);
                bed.cleaningTime = null;
                bed.inRelocation = false;
                bed.patient = null;
                db.SaveChanges();
            }   
        }


        [TestMethod()]
        public void AcceptRelocationTest()
        {
            BVContext db = new BVContext();
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, EStation.Orthopaedie);
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
            Patient pat = new Patient("Peter", "Enis", new DateTime(), false, EStation.Orthopaedie);
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

            pat = new Patient("Peter", "Enis", new DateTime(), false, EStation.Orthopaedie);
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

            pat = new Patient("Peter", "Enis", new DateTime(), false, EStation.Orthopaedie);
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

        [TestMethod()]
        public void DisplayPatientTest()
        {
            BVContext db = new BVContext();
            Controller control = new Controller();

            Patient pat = new Patient("Maxine", "Musterfrau", DateTime.Now, true, EStation.Orthopaedie);
            Bed bed = db.Beds.Find(1);
            bed.station = (int)EStation.Gynaekologie;
            bed.patient = pat;
            db.SaveChanges();

            var bedResult = db.Beds.Where(b => b.patient.firstname == "Maxine").FirstOrDefault();
            Assert.AreEqual(control.DisplayPatient(bedResult.bedId).GetPatient().firstname, "Maxine");

            this.ClearDB();
        }

        [TestMethod()]
        public void GetBettListTest()
        {
            BVContext db = new BVContext();
            Controller control = new Controller();

            Bed bed1 = db.Beds.Find(1);
            bed1.cleaningTime = new DateTime().ToString();
            bed1.station = (int)EStation.Gynaekologie;
            bed1.patient = null;

            Bed bed2 = db.Beds.Find(2);
            bed2.cleaningTime = new DateTime().ToString();
            bed2.station = (int)EStation.Innere_Medizin;
            bed2.patient = new Patient("Lassmiranda", "Dennsiewillja", DateTime.Now, true, EStation.Orthopaedie);

            Bed bed3 = db.Beds.Find(3);
            bed3.cleaningTime = new DateTime().ToString();
            bed3.station = (int)EStation.Orthopaedie;
            bed3.patient = new Patient("Haldie", "Klappe", DateTime.Now, false, EStation.Orthopaedie);

            db.SaveChanges();

            List<IBedView> bedList = control.GetBettList();

            Assert.AreEqual(bedList[bedList.Count-3].GetStation(), EStation.Gynaekologie);
            Assert.AreEqual(bedList[bedList.Count - 2].GetPatient().firstname, "Lassmiranda");
            Assert.AreEqual(bedList[bedList.Count - 1].GetPatient().lastname, "Klappe");

            this.ClearDB();
        }

        [TestMethod()]
        public void SearchPatientTest()
        {
            BVContext db = new BVContext();
            Controller control = new Controller();

            Bed bed1 = db.Beds.Find(1);
            bed1.cleaningTime = new DateTime().ToString();
            bed1.station = (int)EStation.Gynaekologie;
            bed1.patient = new Patient("Gute", "Miene", DateTime.Now, true, EStation.Orthopaedie);

            Bed bed2 = db.Beds.Find(2);
            bed2.cleaningTime = new DateTime().ToString();
            bed2.station = (int)EStation.Innere_Medizin;
            bed2.patient = new Patient("Gute", "Miene", new DateTime(), true, EStation.Orthopaedie);

            Bed bed3 = db.Beds.Find(3);
            bed3.cleaningTime = new DateTime().ToString();
            bed3.station = (int)EStation.Orthopaedie;
            bed3.patient = new Patient("Gute", "nTag", DateTime.Now, false, EStation.Orthopaedie);

            db.SaveChanges();

            //search for existing whole name
            List<IBedView> bedList = control.SearchPatient("Gute Miene");
            Assert.AreEqual(bedList[0].GetPatient().firstname, "Gute");
            Assert.AreEqual(bedList[1].GetPatient().lastname, "Miene");
            Assert.AreEqual(bedList.Count, 2);

            //search non existing whole name
            bedList = control.SearchPatient("Gute Mine");
            Assert.AreEqual(bedList.Count, 0);

            //search for firstname
            bedList = control.SearchPatient("Gute");
            Assert.AreEqual(bedList.Count, 3);

            //search for firstname
            bedList = control.SearchPatient("Miene");
            Assert.AreEqual(bedList.Count, 2);

            //search by patID
            var pat = db.Patients.Where(p => p.lastname == "nTag").FirstOrDefault();
            bedList = control.SearchPatient(pat.patId.ToString());
            Assert.AreEqual(bedList[0].GetPatient().lastname, "nTag");
            Assert.AreEqual(bedList.Count, 1);

            this.ClearDB();
        }

        [TestMethod()]
        [ExpectedException(typeof(BedException))]
        public void DismissPatientTest()
        {
            BVContext db = new BVContext();
            Controller control = new Controller();


            //normal dismiss test
            Bed bed1 = db.Beds.Find(1);
            bed1.cleaningTime = new DateTime().ToString();
            bed1.station = (int)EStation.Gynaekologie;
            bed1.patient = new Patient("Julius", "Caesar", DateTime.Now, true, EStation.Orthopaedie);
            bed1.cleaningTime = null;
            db.SaveChanges();
            int histItemID = bed1.patient.history.historyItem[0].historyItemId;
            control.DismissPatient(bed1.bedId);
            db = new BVContext();
            var bedResult = db.Beds.Where(b=>b.patient.firstname == "Julius").FirstOrDefault();
            Assert.IsNull(bedResult, "bed1 testdata existiert noch in der DB");
            var histItem = db.HistoryItems.Find(histItemID);
            Assert.IsNull(histItem, "history Item existiert noch");

            
            //relocation dismiss test
            Patient pat = new Patient("Thor", "mit dem Hammer", DateTime.Now, false, EStation.Orthopaedie);
            Bed sourceBed = db.Beds.Find(2);
            sourceBed.station = 1;
            sourceBed.patient = pat;
            Relocation Rel = new Relocation(sourceBed, EStation.Paediatrie);
            Bed destBed = db.Beds.Find(3);
            destBed.patient = null;
            destBed.station = (int)EStation.Paediatrie;
            Rel.SetActive(destBed);
            Rel.SetAccepted();
            db.Relocations.Add(Rel);
            db.SaveChanges();
            int relID = Rel.relocationId;
            control.DismissPatient(sourceBed.bedId);
            db = new BVContext();
            destBed = db.Beds.Find(destBed.bedId);
            Assert.IsFalse(destBed.inRelocation);
            bedResult = db.Beds.Where(b => b.patient.firstname == "Thor").FirstOrDefault();
            Assert.IsNull(bedResult, "relBed testdata existiert noch in der DB");
            Rel = db.Relocations.Find(relID);
            Assert.IsNull(Rel);


            //Exception
            Bed bed2 = db.Beds.Find(4);
            bed2.cleaningTime = new DateTime().ToString();
            bed2.station = (int)EStation.Innere_Medizin;
            bed2.patient = null;
            db.SaveChanges();
            control.DismissPatient(bed2.bedId);

            this.ClearDB();
        }

        [TestMethod()]
        public void AddPatientTest()
        {
            Controller cont = new Controller();
            IBedView Bed = cont.AddPatient("Peter", "Enis", EStation.Innere_Medizin, new DateTime(1990, 5, 5), false);
            BVContext db = new BVContext();
            Bed test = db.Beds.Find(Bed.GetBedId());
            Assert.AreEqual("Peter", test.GetPatient().GetFirstName());
            Assert.AreEqual(EStation.Innere_Medizin, test.GetStation());
            Bed = cont.AddPatient("Petra", "Enis", EStation.Gynaekologie, new DateTime(1990, 5, 5), true);
            db = new BVContext();
            test = db.Beds.Find(Bed.GetBedId());
            Assert.AreEqual("Petra", test.GetPatient().GetFirstName());
            Assert.AreEqual(EStation.Gynaekologie, test.GetStation());
            Bed = cont.AddPatient("Petra", "Enis", EStation.Orthopaedie, new DateTime(1990, 5, 5), true);
            db = new BVContext();
            test = db.Beds.Find(Bed.GetBedId());
            Assert.AreEqual("Petra", test.GetPatient().GetFirstName());
            Assert.AreEqual(EStation.Orthopaedie, test.GetStation());
            Bed = cont.AddPatient("Peter", "Enis", EStation.Paediatrie, new DateTime(2004, 5, 5), true);
            db = new BVContext();
            test = db.Beds.Find(Bed.GetBedId());
            Assert.AreEqual("Peter", test.GetPatient().GetFirstName());
            Assert.AreEqual(EStation.Paediatrie, test.GetStation());
            for(int i = 0;i<49;i++)
            {
                Bed = cont.AddPatient("Peter", "Enis", EStation.Paediatrie, new DateTime(2004, 5, 5), true);
            }
            Bed = cont.AddPatient("Peter", "Enis", EStation.Paediatrie, new DateTime(2004, 5, 5), true);
            db = new BVContext();
            test = db.Beds.Find(Bed.GetBedId());
            Assert.AreEqual("Peter", test.GetPatient().GetFirstName());
            Assert.AreEqual(EStation.Gynaekologie, test.GetStation());

        }
    }
}
