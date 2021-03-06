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
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Verlegungs- oder ErzeugungsItem das zur Patientenhistory gehört
    /// </summary>
    [Serializable]
    public class HistoryItem
    {

        public string text
        {
            get;
            set;
        }
        [Key]
        public int historyItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Ein History-Item für die Patientenaufnahme wird erstellt
        /// </summary>
        /// <returns>das erstellte HistoryItem (für HistoryItemListe in History Objekt)</returns>
        public static HistoryItem CreateEntryItem()                                    
        {
            DateTime dat1 = DateTime.Now;
            CultureInfo culture = new CultureInfo("de-DE");     //German Date format
            String patRegisteredText = dat1.ToString("g", culture) + "    Patient aufgenommen";
            HistoryItem historyItem = new HistoryItem(patRegisteredText);
            return historyItem;
        }

        /// <summary>
        /// Ein History-Item für eine Verlegung wird erstellt
        /// </summary>
        /// <param name="sourceBed">Quellbett ID</param>
        /// <param name="destBed">Zielbett ID</param>
        /// <returns>das erstellte HistoryItem (für HistoryItemListe in History Objekt)</returns>
        public static HistoryItem CreateRelocationItem(int sourceBed, int destBed)   
        {
            DateTime dat1 = DateTime.Now;
            CultureInfo culture = new CultureInfo("de-DE");     //German Date format
            String relocText = dat1.ToString("g", culture) + "    Verlegung des Patienten von Bett " + sourceBed + " nach Bett " + destBed;
            HistoryItem historyItem = new HistoryItem(relocText);
            return historyItem;
        }

        public HistoryItem()
        {

        }

        /// <summary>
        ///  Konstruktor für HistoryItem. Wird nur von den beiden oberen statischen Methoden genutzt.
        /// </summary>
        /// <param name="text">text der im attribut abgelegt wird</param>
        public HistoryItem(string text)                                                        
        {
            this.text = text;//Text und id werden gespeichert sowie die aktuelle Zeit in timestamp.
        }

        public string GetText()
        {
            return text;
        }
    }
}

