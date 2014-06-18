﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Wenn der Code neu generiert wird, gehen alle Änderungen an dieser Datei verloren
// </auto-generated>
//------------------------------------------------------------------------------
namespace Bettenverwaltung
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    public class Controller : IController                            //Controller für die Kommunikation zwischen View und Anwendung.
	{
		public virtual BVContext bvContext
		{
			get;
			set;
		}

        public Controller()
        {
           
        }
        //Legt ein Patientenobjekt an und weist diesem mit FindeBedfor ein Bett zu.
        //Wird von FindBed for null zurückgegeben, wird ein Krankenwagen gerufen.
        //Bei Erfolg wird die Bettenid des Bettes, in dem der Patient nun liegt, zurückgegeben.
        //Stimmt die Station des Bettes, welches FindBedFor zurückgibt, und die Zielstation nicht überein muss eine Relocation angelegt werden.
        public virtual IBedView AddPatient(string firstname, string lastname, EStation station, DateTime birthday, bool isFemale)
        {
            bvContext = new BVContext();
            Patient p = new Patient(firstname, lastname, birthday, isFemale, station);
            Bed bed = FindBedFor(p, station);
            if(bed == null)
            {
                return null;
            }
            bed.SetPatient(p);
            if(bed.GetStation() != station)             //anlegen einer Verlegung falls nötig.
            {
                CreateRelocation(bed.GetBedId(), station);
            }
            bvContext.SaveChanges();
            return bed;
        }

        public virtual IBedView DismissPatient(int bedId)               //Der Patient in dem Angegebenen Bett, sowie dessen Historie werden aus der DB gelöscht
        {                                                               //Bei Erfolg wird das angegebene Bett zurückgegeben und die CleaningTime gesetzt.
                                                                        //Falls das Bett leer ist wird eine Exception geworfen.
                                                                        //Falls es gerade eine Angenommene verlegung für dieses Bett gibt, muss diese abgebrochen werden (setUnaccepted) um das Zielbett zu entsperren.
                                                                        //Falls es eine Verlegung mit diesem Bett als SourceBed gibt muss diese aus der Datenbank gelöscht werden.
            bvContext = new BVContext();
            Bed bed = bvContext.Beds.Find(bedId);
            if (bed.GetPatient() == null)
            {
                String str = "Bett " + bed.GetBedId() + " ist bereits leer.";
                throw new BedException(str);
            }
            else
            {
                //look for an active relocation
                var reloc = bvContext.Relocations.Where(r => r.sourceBed.bedId == bed.bedId).FirstOrDefault();
                if (reloc != null)
                {
                    if (reloc.IsAccepted())
                    {
                        reloc.SetUnaccepted();
                        //delete relocation from DB
                    }
                    bvContext.Relocations.Remove(reloc);
                }

                

                //remove patient from bed
                Patient patToRemove = bed.RemovePatient();
                //delete History and HistoryItems from DB
                var history = bvContext.Histories.Find(patToRemove.history.historyId);
                int size = history.GetSize();
                for (int i = 0; i < size; i++ )
                {
                    bvContext.HistoryItems.Remove(history.GetHistoryItem(0));
                }
                bvContext.Histories.Remove(history);

                //delete patient from DB
                bvContext.Patients.Remove(patToRemove);
                bed.StartCleaning();
                bvContext.SaveChanges();
            }
            return (IBedView)bed;
        }

		
        public virtual void AcceptRelocation(int relocationId)          //Die Verlegung wird angenommen (noch nicht ausgeführt!). Das Relocation-Objekt mit der Angegebenen ID wird gesucht
		{
            bvContext = new BVContext();                                //und dessen annaheme-Funktion ausgefürht.
            Relocation Rel = GetRelocation(relocationId);
            Rel.SetAccepted();
            bvContext.SaveChanges();		
        }

		public virtual IBedView DisplayPatient(int bedId)               //Das mit bedId angegeben Bett wird aus der Datenbank geholt und zurückgegeben
		{
            bvContext = new BVContext();
            IBedView bed = (IBedView) bvContext.Beds.Find(bedId);
            return bed;
		}

		public virtual List<IBedView> SearchPatient(string term)        //Der Term wird überprüft ob es sicht um eine Zahl (Pat. ID) oder eine Buchstabenkette(Vorname,Nachname) handelt
		{                                                               //Es wird eine Liste an Betten zurückgegeben, die den Patienten enthalten, deren Name oder Pat. ID mit dem
                                                                        //Suchterm übereinstimmen. Wird nichts gefunden wird eine leere Liste zurückgegeben.
            bvContext = new BVContext();
            List<IBedView> resultList = new List<IBedView>();
            term = term.Trim();
            int patID;
            bool isNumber = int.TryParse(term, out patID);
            if (isNumber)
            {
                //search for patID
                resultList = bvContext.Beds.Where(b => b.patient.patId == patID).ToList<IBedView>();
            }
            else
            {
                //search for name
                var names = term.Split(new char[] {' '}, 2);
                if (names.Count() > 1)
                {
                    string firstName = names[0];
                    string lastName = names[1];

                    resultList = bvContext.Beds.Where(b => b.patient.firstname == firstName && b.patient.lastname == lastName).ToList<IBedView>();
                }
                else
                {
                    resultList = bvContext.Beds.Where(b => b.patient.firstname == term || b.patient.lastname == term).ToList<IBedView>();
                }
            }

            return resultList;
		}

		public virtual void ConfirmRelocation(int relocationId)        //Die Rückverlegung mit der angegebenen ID wird bestätigt. Das passende Rückverlegungsobjekt wird in der Datenbank
		{                                                               //gesucht und die Execute-Funktion aufgerufen
            bvContext = new BVContext();
            Relocation Rel = GetRelocation(relocationId);
            Rel.ExecuteRelocation();
            bvContext.SaveChanges();
		}

        public virtual void CancelRelocation(int relocationId)         //Die Rückverlegung mit der angegebenen ID wird abgebrochen. Das passende Rückverlegungsobjekt wird in der Datenbank
        {                                                               //gesucht und dessen Cancel-Funktion aufgerufen.
            bvContext = new BVContext();
            Relocation Rel = GetRelocation(relocationId);
            Rel.SetUnaccepted();
            bvContext.SaveChanges();
		}

        public virtual List<Relocation> GetActiveRelocationList()        //Die Liste aller aktiven Rückverlegungen wird aus der Datenbank geholt und zurückgegeben.
		{
            bvContext = new BVContext();
            var Rels = bvContext.Relocations.Where(R => R.destinationBed != null);
            List<Relocation> LRels = new List<Relocation>(Rels.ToArray());
            return LRels;

		}

		private Relocation CreateRelocation(int bedId, EStation station)    //Eine Relocation mit für das Angegebene bett in die Zielstation station wird erstellt und zurückgegeben. Wird beim Anlegen eines Patienten eventuell Aufgerufen.
		{                                                                   
            Relocation Rel = new Relocation(bvContext.Beds.Find(bedId), station);
            bvContext.Relocations.Add(Rel);
            return Rel;
		}

		public virtual List<IBedView> GetBettList()                         //Eine Liste aller Betten wird aus der Datenbank geholt und zurückgegeben.
		{
            bvContext = new BVContext();
            List<IBedView> bedList = new List<IBedView>(bvContext.Beds.ToList());
            return bedList;
		}


        //Findet ein passendes Bett für den Patienten. Die Daten werde zunächst auf plausibilität geprüft. Falls der angegebene Patient nicht
		//in die Angegebene Station gelegt werden kann, wird eine Exception geworfen. Ist die Station voll, wird der Patient nach den Vorgaben
		//im Lasten/Pflichtenheft verlegt. Wird kein Bett gefunden wird null zurückgegeben (Krankenwagen!!)
		//Die Funktion prüft ob für ein gefundenes Bett Ziel eine Rückverlegung ist und falls ja wird versucht ein neues Bett in der selben Station zu finden
        //wird kein anderes Bett gefunden wird die Rückverlegung auf Inaktiv gesetzt und das gefundene Bett zurückgegeben.
        private Bed FindBedFor(Patient p, EStation station)
        {
            
            List<EStation> stations = new List<EStation>();         //List wird mit der übergeben Station (als erstes Element!) und allen weiteren Stationen in die der Patient verlegt werden darf gefüllt
            System.Linq.IQueryable<Bettenverwaltung.Bed> Beds;
            List<Relocation> rel = this.GetActiveRelocationList();  //Holt die aktiven Verlegungen aus der DB.
            switch (station)
            {
                case EStation.Gynaekologie:
                    if (p.GetAge() < 12)
                    {
                        throw new BedException("Kinder müssen in die Kinderklinik.");
                    }
                    if (!p.isFemale)
                    {
                        throw new BedException("Ein männlicher Patient kann nicht in die Gynäkologie gelegt werden.");
                    }
                    stations.Add(EStation.Gynaekologie);
                    stations.Add(EStation.Innere_Medizin);
                    stations.Add(EStation.Orthopaedie);
                    break;
                case EStation.Innere_Medizin:
                    if (p.GetAge() < 12)
                    {
                        throw new BedException("Kinder müssen in die Kinderklinik.");
                    }
                    stations.Add(EStation.Innere_Medizin);
                    stations.Add(EStation.Orthopaedie);
                    if(p.isFemale)
                    {
                        stations.Add(EStation.Gynaekologie);
                    }
                    break;
                case EStation.Orthopaedie:
                    if (p.GetAge() < 12)
                    {
                        throw new BedException("Kinder müssen in die Kinderklinik.");
                    }
                    stations.Add(EStation.Orthopaedie);
                    stations.Add(EStation.Innere_Medizin);
                    if (p.isFemale)
                    {
                        stations.Add(EStation.Gynaekologie);
                    }
                    break;
                case EStation.Paediatrie:
                    if(p.GetAge() > 12)
                    {
                        throw new BedException("Nur Kinder dürfen in die Pädiatrie.");
                    }
                    stations.Add(EStation.Paediatrie);
                    stations.Add(EStation.Gynaekologie);
                    break;
                                                                            //stations wurde nun mit möglichen Stationen gefüllt.
            }   
            do                                                              //die Stationen werden durchgegangen bis eine Station mit mind einem freien Bett gefunden wurde.
            {
                int istation = (int)(stations.FirstOrDefault());
                Beds = bvContext.Beds.Where(B => B.station == istation).Where(B => B.patient == null).Where(B => B.inRelocation == false).Where(B => B.cleaningTime == null);
                stations.RemoveAt(0);
            } while (Beds.ToList().Count == 0 && stations.Count != 0);

            if(Beds.ToList().Count == 0)           //falls keine freie Station gefunden wurde
            {
                return null;
            }
            foreach(Bed b in Beds)                  //jedes Bett wird nun überprüft ob es Ziel einer Verlegung ist.
            {
                bool check = false;                 //wird true wenn das Bett Ziel einer Verlegung ist.
                foreach(Relocation r in rel)        
                {
                    if(r.GetDestinationBed().GetBedId() == b.GetBedId())    //ist das bett Ziel der Verlegung?
                    {
                        check = true;
                        break;
                    }
                }
                if(check!=true)                     //falls das Bett nicht Ziel einer Verlegung ist.
                {
                    return b;
                }
            }

            Bed ret = Beds.ToArray()[0];  //fall alle Betten Ziel einer Verlegung. Nimm das erste Bett.
            foreach (Relocation r in rel)  //suche Verlegung mit dem Bett als Ziel
            {
                if (r.GetDestinationBed().GetBedId() == ret.GetBedId())    //ist das bett Ziel der Verlegung?
                {
                    r.SetInactive();        //setze Verlegung inaktiv.
                    break;
                }
            }
            return ret;
        }                                              
			                                                            

		private int GetNextHistoryId()                                  //Die getNext****Id Funktionen, suchen sich das jeweilige Objekt mit der höchsten ID aus der DB und geben diese ID inkrementiert zurück
		{
			throw new System.NotImplementedException();
		}

		private int GetNextHistoryItemId()
		{
			throw new System.NotImplementedException();
		}

		private int GetNextRelocationId()
		{
			throw new System.NotImplementedException();
		}

		private int GetNextBedId()
		{
			throw new System.NotImplementedException();
		}

		private int GetNextPatientId()
		{
			throw new System.NotImplementedException();
		}

        private Relocation GetRelocation(int relId)                        //Die Relocation mit der Angegebenen ID wird aus der Datenbank gesucht und zurückgegeben.
        {
            Relocation Rel = bvContext.Relocations.Find(relId);
            if (Rel == null)
            {
                throw new BedException("Es gibt keine Verlegung mit der ID " + relId);
            }
            return Rel;
        }
	}
}

