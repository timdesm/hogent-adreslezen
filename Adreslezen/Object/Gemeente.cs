using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Gemeente
    {

        public String Gemeentenaam { get; set; }
        public int NIScode { get; set; }

        public Gemeente(int nsicode, String gemeentenaam)
        {
            this.NIScode = nsicode;
            this.Gemeentenaam = gemeentenaam;
        }
    }
}
