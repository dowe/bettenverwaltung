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

	public class HistoryItem
	{
		private DateTime timestamp
		{
			get;
			set;
		}

		private string text
		{
			get;
			set;
		}

		private int historyItemId
		{
			get;
			set;
		}

		public static HistoryItem CreateEntryItem(int historyItemId)                                    //Ein History-Item für die Patientenaufnahme wird erstellt
		{
			throw new System.NotImplementedException();
		}

		public static HistoryItem CreateRelocationItem(int historyItemId, int sourceBed, int destBed)   //Ein History-Item für eine Verlegung wird erstellt
		{
			throw new System.NotImplementedException();
		}

		public HistoryItem(int id, string text)                                                         //Konstruktor für HistoryItem. Wird nur von den beiden oberen statischen Methoden genutzt.
		{                                                                                               //Text und id werden gespeichert sowie die aktuelle Zeit in timestamp.
		}

	}
}

