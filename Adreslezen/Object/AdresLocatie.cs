﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class AdresLocatie
    {

        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public AdresLocatie(double x, double y)
        {
            this.ID = 0;
            this.X = x;
            this.Y = y;
        }

        public AdresLocatie(int id, double x, double y)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;
            AdresLocatie location = (AdresLocatie) obj;
            if (this.X == location.X && this.Y == location.Y) return true;
            return false;
        }
    }
}
