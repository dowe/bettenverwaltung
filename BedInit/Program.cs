using Bettenverwaltung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedInit
{
    class Program
    {
        static void Main(string[] args)
        {
            BVContext db = new BVContext();
            for (int i = 1; i <= 200; i++)
            {
                Bed bed = new Bed();
                if (i > 0 && i < 51)
                {
                    bed.station = (int)EStation.Paediatrie;
                }
                else if (i > 50 && i < 101)
                {
                    bed.station = (int)EStation.Gynaekologie;
                }
                else if (i > 100 && i < 151)
                {
                    bed.station = (int)EStation.Innere_Medizin;
                }
                else if (i > 150 && i < 201)
                {
                    bed.station = (int)EStation.Orthopaedie;
                }
                else if (i < 1 || i > 200)
                {
                    Console.WriteLine("Bett Id out of Range");
                }
                db.Beds.Add(bed);
            }
            db.SaveChanges();
            Console.WriteLine("Beds der DB hinzugefuegt");
        }
    }
}
