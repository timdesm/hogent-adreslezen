using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adreslezen
{
    class Adres
    {
        public int ID { get; set; }

        public String AppNumber { get; set; }

        public String BusNumber { get; set; }

        public String Number { get; set; }

        public String NumberLabel { get; set; }

        public AdresLocatie Locatie { get; set; }

        public int PostCode { get; set; }

        public Straatnaam Straatnaam { get; set; }

        public Gemeente Gemeente { get; set; }


        public Adres(int id, Straatnaam straatnaam, String appNumber, String busNumber, String number, String numberLabel, Gemeente gemeente, int postcode, double x, double y)
        {
            this.ID = id;
            this.AppNumber = appNumber;
            this.BusNumber = busNumber;
            this.Number = number;
            this.NumberLabel = numberLabel;
            this.Straatnaam = straatnaam;
            this.PostCode = postcode;
            this.Gemeente = gemeente;
            this.addLocation(x, y);
        }

        public void addLocation(double x, double y)
        {
            AdresLocatie location = new AdresLocatie(x, y);
            //foreach (AdresLocatie loc in Program.locations.Values)
            //{
            //    if(loc.Equals(location))
            //    {
            //        this.Locatie = loc;
            //        break;
            //}
            
            int key = 1;
            if(Program.locations.Count > 0)
            {
                key = Program.locations.Keys.Last() + 1;
            }

            location.ID = key;
            this.Locatie = location;
            Program.locations.Add(key, location);
        }

        public override string ToString()
        {
            return "Adres: #" + this.ID + " , " + this.Straatnaam.Streetname + " , " + this.Number + " , " + this.PostCode + " , (" + this.Locatie.X + " x " + this.Locatie.Y + ")";
        }
    }
}
