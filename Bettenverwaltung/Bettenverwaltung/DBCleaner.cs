//------------------------------------------------------------------------------
//Diese Klasse wird einmalig als Thread ausgeführt. Sie schließt die Säuberung der Betten nach 2 Stunden ab
//und bricht Rückverlegungen ab, die schon zu lange angenommen aber nicht abgeschlossen sind.
//------------------------------------------------------------------------------
namespace Bettenverwaltung
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

    public class DBCleaner
    {

        public static readonly TimeSpan TIMEOUT_CLEANING = new TimeSpan(0, 0, 5);
        public static readonly TimeSpan TIMEOUT_REMOVE_FORGOTTEN_RELOCATION = new TimeSpan(0, 30, 0);
        private Timer t;
        private BVContext db;

        /// <summary>
        /// Erstellt einen neuen DBCleaner
        /// </summary>
        /// <param name="time">Abstand zwischen den einzelnen Cleaning Events in Sekunden</param>
        public DBCleaner(double time)
        {
            t = new Timer(time * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Clean);
        }

        public virtual void Start()
        {
            t.Start();
        }

        public virtual void Stop()
        {
            t.Stop();
        }

        /// <summary>
        /// Diese Funktion wird als Thread ausgeführt. Sie überprüft die Säuberungs-Timestamps der Bettenobjekte und 
        /// schließt die Säuberung ab falls 2 Stunden vergangen sind. Je nachdem müssen dann noch inaktive Rückverlegungsobjekte auf aktiv gesetzt werden.  
        /// Dann werden alle angenommen Rückverlegungsobjekte überprüft und abgebrochen falls die Zeit überschritten wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clean(object sender, System.Timers.ElapsedEventArgs e)
        {
            db = new BVContext();           // lege immer neuen DBContext an um Änderungen an der db zu sehen
            this.CleanBeds();
            this.RemoveForgottenAcceptedRelocations();
        }

        /// <summary>
        /// Geht alle Betten mit Cleaning Time durch und stoppt die Säuberung wenn mehr als 2 Stunden vergangen sind
        /// </summary>
        private void CleanBeds()
        {
            var beds = db.Beds.Where(b => b.cleaningTime != null);
            List<Bed> freedBeds = new List<Bed>();
            // set beds free
            foreach (var bed in beds)
            {
                if ((DateTime.Now - bed.GetCleaningTime()) > TIMEOUT_CLEANING)
                {
                    bed.StopCleaning();
                    freedBeds.Add(bed);
                }
            }
            db.SaveChanges();
            // no multiple active result sets anymore as it did not allow to save the changes in SetRelocationActive
            foreach (Bed bed in freedBeds)
            {
                SetRelocationActive(bed);
            }
        }

        /// <summary>
        /// Findet zur frei gewordenen Station passende Relocations und setzt diese ggf. "active"
        /// </summary>
        /// <param name="bed"></param>
        private void SetRelocationActive(Bed bed)
        {
            Relocation relocation = db.Relocations.Where(r => r.sourceBed.patient.correctStation == bed.station).Where(r => r.destinationBed == null).FirstOrDefault();
            if (relocation != null)
            {
                relocation.SetActive(bed);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Setzt vergessene Relocations nach einiger Zeit wieder auf Unaccepted, so dass diese erneut angenommen werden können
        /// </summary>
        private void RemoveForgottenAcceptedRelocations()
        {
            var relocations = db.Relocations.Where(r => r.timestamp != null && r.destinationBed != null && r.accepted == true);
            foreach (var relocation in relocations)
            {
                if ((DateTime.Now - relocation.GetTimestamp()) > TIMEOUT_REMOVE_FORGOTTEN_RELOCATION)
                {
                    relocation.SetUnaccepted();
                }
            }
            db.SaveChanges();
        }
    }
}

