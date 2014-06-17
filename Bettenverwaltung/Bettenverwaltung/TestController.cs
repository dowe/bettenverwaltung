using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bettenverwaltung
{
    public class TestController : IController
    {
        private List<Bed> beds;
        private List<Relocation> relocations;
        private List<Patient> patients;

        public TestController()
        {
            MockInitBeds();
            MockInitPatients();
        }

        private void MockInitBeds()
        {
            beds = new List<Bed>();
            for (int i = 1; i <= 200; i++)
            {
                Bed bed = new Bed();
                if (i > 0 && i < 51)
                {
                    bed.station = (int)EStation.Paediatrie;
                }
                else if (i > 50 && i < 101)
                {
                    bed.station = (int)EStation.Gynaekologie;
                }
                else if (i > 100 && i < 151)
                {
                    bed.station = (int)EStation.Innere_Medizin;
                }
                else if (i > 150 && i < 201)
                {
                    bed.station = (int)EStation.Orthopaedie;
                }
                else if (i < 1 || i > 200)
                {
                    Console.WriteLine("Bett Id out of Range");
                }
                bed.bedId = i;
                beds.Add(bed);
            }
        }

        private void MockInitPatients()
        {
            patients = new List<Patient>();
            Patient pat = new Patient("Paul", "Enis", DateTime.Now, false, EStation.Orthopaedie);
            pat.patId = 1;
            patients.Add(pat);
            beds[105].SetPatient(pat);
            relocations = new List<Relocation>();
            Relocation rel = new Relocation(beds[105], EStation.Orthopaedie);
            rel.relocationId = 1;
            rel.SetActive(beds[160]);
            relocations.Add(rel);
        }

        public void AcceptRelocation(int relocationId)
        {
            throw new NotImplementedException();
        }

        public IBedView AddPatient(string firstname, string lastname, EStation station, DateTime birthday, bool isFemale)
        {
            throw new NotImplementedException();
        }

        public void CancelRelocation(int realocationId)
        {
            throw new NotImplementedException();
        }

        public void ConfirmRelocation(int realocationId)
        {
            throw new NotImplementedException();
        }

        public IBedView DismissPatient(int bedId)
        {
            throw new NotImplementedException();
        }

        public IBedView DisplayPatient(int bedId)
        {
            return beds[bedId - 1];
        }

        public List<Relocation> GetActiveRelocationList()
        {
            List<Relocation> res = new List<Relocation>();
            foreach (Relocation rel in relocations)
            {
                if (rel.IsActive())
                {
                    res.Add(rel);
                }
            }
            return res;
        }

        public List<IBedView> GetBettList()
        {
            return new List<IBedView>(beds);
        }

        public List<IBedView> SearchPatient(string term)
        {
            throw new NotImplementedException();
        }
    }
}