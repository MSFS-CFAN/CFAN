using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Mods
{
    class Scenery : Mod
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
}
