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
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Kontext für DB-Zugriff
    /// </summary>
    public class BVContext : DbContext
    {
        public BVContext()
            :base("BVContext")
        {

        }
        public virtual DbSet<Relocation> Relocations
        {
            get;
            set;
        }

        public virtual DbSet<Bed> Beds
        {
            get;
            set;
        }

        public virtual DbSet<Patient> Patients
        {
            get;
            set;
        }

        public virtual DbSet<History> Histories
        {
            get;
            set;
        }

        public virtual DbSet<HistoryItem> HistoryItems
        {
            get;
            set;
        }
    }
}

