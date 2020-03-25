using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Straatnaam
    {
        public Gemeente Gemeente { get; set; }
        public int ID { get; set; }

        public String Streetname { get; set; }

        public Straatnaam(int ID, String straatnaam, Gemeente gemeente)
        {
            this.ID = ID;
            this.Streetname = straatnaam;
            this.Gemeente = gemeente;
        }
    }
}
