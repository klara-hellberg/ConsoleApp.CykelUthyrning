using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.CykelUthyrning
{
    // Klass för kundinformation (Krav: Kund klass)
    public class Kund
    {
        public string Namn { get; set; }
        public int Alder { get; set; }
        public bool ArMedlem { get; set; }
        public bool ArPersonal { get; set; } // Extra funktion för personalrabatt

        public Kund(string namn, int alder, bool arMedlem, bool arPersonal)
        {
            Namn = namn;
            Alder = alder;
            ArMedlem = arMedlem;
            ArPersonal = arPersonal;
        }
    }
}
