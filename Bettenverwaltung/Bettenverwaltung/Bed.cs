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

   
	public class Bed : IBedCleaner, IBedView, IBedRelocation      //Datenbankobjekt für die Betten des KHs
	{

        public int bedId
        {
            get;
            set;
        }

        public int station                                     //0 Innere_Medizin 1 Orthopädie 2 Pediatrie 3 Gynäkologie
		{
			get;
			set;
		}

        public bool inRelocation
		{
			get;
			set;
		}

        public DateTime? cleaningTime
		{
			get;
			set;
		}

		public virtual Patient Patient
		{
			get;
			set;
		}

		public virtual void SetPatient(Patient patient)             //legt eine Patienten in das Bett, Exception falls sich schon ein Patient in dem Bett
		{                                                           //befindet oder es gesperrt ist.
            if (this.Patient == null && this.cleaningTime == null)
            {
                this.Patient = patient;
            }
            else
            {
                String str = "Bett " + this.bedId + " bereits belegt";
                throw new BedException(str);
            }
		}

		public virtual Patient RemovePatient()
		{
            if (this.Patient != null)
            {
                Patient temp = this.Patient;
                this.Patient = null;
                return temp;
            }
            else
            {
                String str = "Bett " + this.bedId + " ist bereits leer.";
                throw new BedException(str);
            }
		}

		public virtual void SetInRelocation(bool status)
		{
            if (this.inRelocation == true && status == true)
            {
                String str = "Bett " + this.bedId + " bereits für Verlegung geplant.";
                throw new BedException(str);
            }
            else
                this.inRelocation = status;
		}

		public virtual void StartCleaning()                     //setzt die cleaningtime des Bettenobjekts. Das Bett ist für diese Zeit gesperrt.
		{

            if (this.cleaningTime == null)
                this.cleaningTime = DateTime.Now;
            else
            {
                String str = "Bett " + this.bedId + " wird bereits gereinigt";
                throw new BedException(str);
            }
		}

		public virtual void StopCleaning()                      //setzt die cleaningtime zurück auf null. Diese Funktion wird nur vom DB_Cleaner verwendet
		{
            if (this.cleaningTime != null)
                this.cleaningTime = null;
            else
            {
                String str = "Bett " + this.bedId + " ist schon sauber";
                throw new BedException(str);
            }
		}

		public virtual DateTime? GetCleaningTime()               //überprüfung der cleaingtime. Wird nur vom DB_Cleaner verwendet.
		{
            return this.cleaningTime;
		}

		public virtual Patient GetPatient()
		{
            return this.Patient;
		}

		public virtual EStation GetStation()                    //Map den Integer der Datenbank auf das Enum EStation (siehe dazu Kommentar der Variable Station oben)
		{
            return (EStation)this.station;
		}

		public virtual int GetBedId()
		{
            return this.bedId;
		}

		public virtual bool IsEmpty()
		{
            bool ret = false;
            if (this.Patient == null)
                ret = true;

            return ret;
		}

		public virtual bool IsGettingCleaned()
		{
            bool ret = true;
            if (this.cleaningTime == null)
                ret = false;
            return ret;
		}

		public virtual bool IsInRelocation()
		{
            return this.inRelocation;
		}

	}
}

