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
        public DBCleaner(double time)
        {
            t = new Timer(time * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(Clean);
            db = new BVContext();
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
            bool cleanedBedFound = false;
            var beds = db.Beds.Where(b => b.cleaningTime != null);
            foreach (var bed in beds)
            {
                if ((DateTime.Now - bed.GetCleaningTime()) > new TimeSpan(2, 0, 0))
                {
                    cleanedBedFound = true;
                    bed.StopCleaning();
                }
            }
            if (cleanedBedFound)
            {
                db.SaveChanges();
            }
        }
    }
}

