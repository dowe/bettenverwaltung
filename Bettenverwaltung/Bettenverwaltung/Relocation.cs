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

	public class Relocation
	{
		public int station
		{
			get;
			set;
		}

		public int relocationId
		{
			get;
			set;
		}

		public bool accepted;

		public DateTime? timestamp
		{
			get;
			set;
		}

		public virtual IBedRelocation sourceBed
		{
			get;
			set;
		}

		public virtual IBedRelocation destinationBed
		{
			get;
			set;
		}

		public Relocation(IBedRelocation bed, EStation station, int relId) //Konstruktor: Die übergebenen Werte werden zugewiesen
		{                                                                  //weitere Werte werden zunächst mit null oder false initialisiert.
            this.sourceBed = bed;
            this.station = (int)station;
            this.relocationId = relId;
            this.destinationBed = null;
            this.accepted = false;
            this.timestamp = null;
		}

        public virtual EStation GetStation()        //Der Interger Wert Station wird auf das Enum EStation gemappt.
        {                                           //Wirft eine BedException falls der Wert nicht zwischen 0 und 3 liegt.
            if(station < 0 || station > 3)
            {
                throw new BedException("Die in der Verlegung eingetragene Station existiert nicht.");
            }
            return (EStation)station;
        }

		public virtual void SetActive(IBedRelocation bed)       //Diese Funktion wird aufgerufen wenn eine Rückverlegung möglich ist.
		{                                                       //Das Zielbett wird eingetragen jedoch noch nicht gesperrt.
			if(!bed.IsEmpty()||bed.IsGettingCleaned()||bed.IsInRelocation())
            {
                throw new BedException("Das Zielbett der Verlegung ist bereits gesperrt.");
            }
            if(this.destinationBed != null)
            {
                throw new BedException("Die Verlegung ist bereits aktiv");
            }
            this.destinationBed = bed;
		}

		public virtual void SetInactive()                   //Das Zielbett wird wieder auf Null gesetzt. Die Rückverlegung ist inaktiv.
		{
            if(this.destinationBed == null)
            {
                throw new BedException("Die Verlegung ist nicht aktiv");
            }
            this.destinationBed = null;
		}

		public virtual bool ExecuteRelocation(int historyItemId) //Führt die Rückverlegung durch.
		{
            if(this.accepted == false || this.destinationBed == null)
            {
                throw new BedException("Die auszuführende Verlegung ist nicht angenommen/aktiv");
            }
            Patient Patient = sourceBed.RemovePatient();         //Patient wird aus dem Bett entfernt und zurückgegeben.
            destinationBed.SetInRelocation(false);
            destinationBed.SetPatient(Patient);
            Patient.GetHistory().InsertHistoryItem(HistoryItem.CreateRelocationItem(historyItemId, sourceBed.GetBedId(), destinationBed.GetBedId()));  //Anlegen eines neuen HistoryItems für den Patienent.
            return true;
        }

		public virtual bool IsActive()                          //abfrage ob die Rückverlegung aktiv ist
		{                                                       //false(inaktiv) falls destBed = null
			if(destinationBed == null)
            {
                return false;
            }
            return true;
		}

		public virtual bool IsAccepted()                    //Abfrage ob Rückverlegung angenommen wurde
		{                                                   
            return accepted;
		}

		public virtual void SetAccepted()                   //die Rückverlegung wird angenommen. Wirft eine Exception falls die Rückverlegung nicht aktiv ist.
		{
            if(this.destinationBed == null)
            {
                throw new BedException("Die Verlegung kann nicht angenommen werden, da sie nicht aktiv ist.");
            }
            this.accepted = true;
            destinationBed.SetInRelocation(true);           //Das Bett wird gesperrt.
            this.timestamp = DateTime.Now;                  //Zeitpunkt der Annahme wird gesetzt.
		}

		public virtual IBedRelocation GetSourceBed()
		{
            return sourceBed;
		}

		public virtual IBedRelocation GetDestinationBed()
		{
            return destinationBed;
		}

		public virtual DateTime? GetTimestamp()
		{
            return timestamp;
		}

		public virtual void SetUnaccepted()                 //die Rückverlegung wird abgebrochen.
		{
            if(this.accepted == false)
            {
                throw new BedException("Die Verlegung ist nicht angenommen.");
            }
            this.accepted = false;
            destinationBed.SetInRelocation(false);          //Das Bett wird entsperrt.
            this.timestamp = null;                          //setzt Annahmezeit auf null.
		}

		public virtual int GetId()
		{
            return this.relocationId;
		}

	}
}

