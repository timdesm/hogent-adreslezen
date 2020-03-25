using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Adres
    {
        public int ID { get; set; }

        public String AppNumber { get; set; }

        public String BusNumber { get; set; }

        public String Number { get; set; }

        public AdresLocatie Locatie { get; set; }

        public int PostCode { get; set; }

        public Straatnaam Straatnaam { get; set; }


        public Adres(int id, Straatnaam straatnaam, String appNumber, String busNumber, String number, Gemeente gemeente, int postcode, double x, double y)
        {
            this.ID = id;
            this.AppNumber = appNumber;
            this.BusNumber = busNumber;
            this.Number = number;
            this.Straatnaam = straatnaam;
            this.PostCode = postcode;
            this.addLocation(x, y);
        }

        public void addLocation(double x, double y)
        {
            AdresLocatie locatie = new AdresLocatie(x, y);
            this.Locatie = locatie;
        }

        public override string ToString()
        {
            return "Adres: #" + this.ID + " , " + this.Straatnaam.Streetname + " , " + this.Number + " , " + this.PostCode + " , (" + this.Locatie.X + " x " + this.Locatie.Y + ")";
        }
    }
}
