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

	public class BVContext
	{
		public virtual DBSet<Relocation> Relocations
		{
			get;
			set;
		}

		public virtual DBSet<Bed> Beds
		{
			get;
			set;
		}

		public virtual DBSet<Patient> Patients
		{
			get;
			set;
		}

		public virtual DBSet<History> Histories
		{
			get;
			set;
		}

		public virtual DBSet<HistoryItem> HistoryItems
		{
			get;
			set;
		}

	}
}

