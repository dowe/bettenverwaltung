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
        Timer t;
        BVContext db;

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
        }

        /// <summary>
        /// Geht alle Betten mit Cleaning Time durch und stoppt die Säuberung wenn mehr als 2 Stunden vergangen sind
        /// </summary>
        private void CleanBeds()
        {
            bool cleanedBedFound = false;
            var beds = db.Beds.Where(b => b.cleaningTime != null);
            foreach (var bed in beds)
            {
                if ((DateTime.Now - bed.GetCleaningTime()) > new TimeSpan(2, 0, 0))
                {
                    cleanedBedFound = true;
                    bed.StopCleaning();
                    this.SetRelocationActive(bed);
                }
            }
            if (cleanedBedFound)
            {
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Findet zur frei gewordenen Station passende Relocations und setzt diese ggf. "active"
        /// </summary>
        /// <param name="bed"></param>
        private void SetRelocationActive(Bed bed)
        {
            Relocation relocation = db.Relocations.Where(r => r.sourceBed.patient.correctStation ==  bed.station).FirstOrDefault();
            if (relocation != null)
            {
                relocation.SetActive(bed);
            }
        }
    }
}

