using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.CykelUthyrning
{
    // Basklass för alla fordon (Krav: Arv)
    public class Fordon
    {
        public int Nummer { get; set; }
        public double TimPris { get; set; }
        public bool ArUthyrd { get; set; }
        public string Typ { get; set; }
        public int AntalHyrtimmar { get; set; }

        public Fordon(int nummer, double timPris, string typ)
        {
            Nummer = nummer;
            TimPris = timPris;
            ArUthyrd = false;
            Typ = typ;
            AntalHyrtimmar = 0;
        }
    }

    // Klass för vanlig cykel som ärver från Fordon
    public class Cykel : Fordon
    {
        public Cykel(int nummer, double timPris) : base(nummer, timPris, "Cykel")
        {
        }
    }

    // Klass för elcykel som ärver från Fordon och har batterinivå
    public class Elcykel : Fordon
    {
        public int BatteriNiva { get; set; } // Procent 0-100

        public Elcykel(int nummer, double timPris, int batteriNiva) : base(nummer, timPris, "Elcykel")
        {
            BatteriNiva = batteriNiva;
        }
    }


}

