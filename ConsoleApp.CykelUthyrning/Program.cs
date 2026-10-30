using System;
using System.Collections.Generic;

namespace ConsoleApp.CykelUthyrning 
{
    // Huvudprogrammet som hanterar menyn och användarens val
    class Program
    {
        static void Main(string[] args)
        {
            // Skapar listor för att spara alla fordon och registrerade kunder 
            List<Fordon> fordonLista = new List<Fordon>();
            List<Kund> kundLista = new List<Kund>();

            // Programmet startar med minst två fordon av varje typ 
            fordonLista.Add(new Cykel(1, 40));
            fordonLista.Add(new Cykel(2, 40));
            fordonLista.Add(new Elcykel(3, 70, 80));
            fordonLista.Add(new Elcykel(4, 70, 25));

            bool koraProgram = true;

            // Huvudloop för menyn som körs tills användaren väljer att avsluta
            while (koraProgram)
            {
                Console.WriteLine("\n=== Kajens Cykeluthyrning ===");
                Console.WriteLine("1. Visa fordon  2. Registrera kund  3. Hyr ut");
                Console.WriteLine("4. Lämna tillbaka  5. Ladda  6. Avsluta");
                Console.Write("Val: ");

                // Try-catch för att fånga upp krascher om användaren skriver bokstäver istället för siffror 
                try
                {
                    int val = Convert.ToInt32(Console.ReadLine());

                    switch (val)
                    {
                        case 1:
                            VisaFordon(fordonLista);
                            break;
                        case 2:
                            RegistreraKund(kundLista);
                            break;
                        case 3:
                            HyrUtFordon(fordonLista, kundLista);
                            break;
                        case 4:
                            LamnaTillbakaFordon(fordonLista);
                            break;
                        case 5:
                            LaddaFordon(fordonLista);
                            break;
                        case 6:
                            koraProgram = false;
                            Console.WriteLine("Programmet avslutas. Tack för idag!");
                            break;
                        default:
                            Console.WriteLine("Ogiltigt val. Välj en siffra mellan 1 och 6.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Felaktig inmatning! Vänligen ange en siffra.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
                }
            }
        }

        // Visar en tabell över alla fordon och deras status
        static void VisaFordon(List<Fordon> fordonLista)
        {
            Console.WriteLine("\nNr\tTyp\t\tStatus\t\tBatteri");
            foreach (var f in fordonLista)
            {
                string status = f.ArUthyrd ? "uthyrd" : "ledig";
                string batteriInfo = "";

                // Om fordonet är en elcykel visar vi även batterinivån
                if (f is Elcykel elcykel)
                {
                    batteriInfo = $"{elcykel.BatteriNiva} %";
                }

                Console.WriteLine($"{f.Nummer}\t{f.Typ}\t\t{status}\t\t{batteriInfo}");
            }
        }

        // Registrerar en ny kund med namn, ålder samt medlems- och personalstatus
        static void RegistreraKund(List<Kund> kundLista)
        {
            Console.Write("Namn: ");
            string namn = Console.ReadLine();

            Console.Write("Ålder: ");
            int alder = Convert.ToInt32(Console.ReadLine());

            Console.Write("Medlem (ja/nej): ");
            bool arMedlem = Console.ReadLine().ToLower() == "ja";

            // Extra funktion: Personalrabatt
            Console.Write("Personal (ja/nej): ");
            bool arPersonal = Console.ReadLine().ToLower() == "ja";

            Kund nyKund = new Kund(namn, alder, arMedlem, arPersonal);
            kundLista.Add(nyKund);

            Console.WriteLine($"{namn} är registrerad.");
        }

        // Hanterar uthyrning med alla tillhörande krav och regler
        static void HyrUtFordon(List<Fordon> fordonLista, List<Kund> kundLista)
        {
            if (kundLista.Count == 0)
            {
                Console.WriteLine("Inga kunder är registrerade ännu. Registrera kund först.");
                return;
            }

            Console.Write("Kundens namn: ");
            string kundNamn = Console.ReadLine();

            Kund aktivKund = kundLista.Find(k => k.Namn.ToLower() == kundNamn.ToLower());
            if (aktivKund == null)
            {
                Console.WriteLine("Kunden hittades inte. Registrera kunden först.");
                return;
            }

            Console.Write("Fordonsnummer: ");
            int fordonsNr = Convert.ToInt32(Console.ReadLine());

            Fordon valtFordon = fordonLista.Find(f => f.Nummer == fordonsNr);
            if (valtFordon == null)
            {
                Console.WriteLine("Fordonet finns inte.");
                return;
            }

            // Regel: Ett uthyrt fordon kan inte hyras ut igen
            if (valtFordon.ArUthyrd)
            {
                Console.WriteLine("Detta fordon är redan uthyrt!");
                return;
            }

            // Regel: Åldersgränser (15 år för cykel, 18 år för elcykel)
            if (valtFordon is Elcykel)
            {
                if (aktivKund.Alder < 18)
                {
                    Console.WriteLine($"{aktivKund.Namn} är {aktivKund.Alder} år och måste vara minst 18 år för att hyra Elcykel.");
                    return;
                }
            }
            else
            {
                if (aktivKund.Alder < 15)
                {
                    Console.WriteLine($"{aktivKund.Namn} är {aktivKund.Alder} år och måste vara minst 15 år för att hyra Cykel.");
                    return;
                }
            }

            // Regel: Elcykel med batteri under 30% kan inte hyras ut
            if (valtFordon is Elcykel ec && ec.BatteriNiva < 30)
            {
                Console.WriteLine($"Elcykel {ec.Nummer} har för lite batteri ({ec.BatteriNiva} %). Ladda den först.");
                return;
            }

            Console.Write("Antal timmar: ");
            int timmar = Convert.ToInt32(Console.ReadLine());

            // Genomför uthyrningen
            valtFordon.ArUthyrd = true;
            valtFordon.AntalHyrtimmar = timmar;

            // Om det är en elcykel förbrukas 10% batteri per hyrd timme
            if (valtFordon is Elcykel elc)
            {
                elc.BatteriNiva -= (timmar * 10);
                if (elc.BatteriNiva < 0) elc.BatteriNiva = 0;
            }

            Console.WriteLine($"{aktivKund.Namn} hyr {valtFordon.Typ} {valtFordon.Nummer} i {timmar} timmar.");
        }

        // Hanterar återlämning, räknar ut pris och applicerar rabatter (Medlem / Personal)
        static void LamnaTillbakaFordon(List<Fordon> fordonLista)
        {
            Console.Write("Fordonsnummer att lämna tillbaka: ");
            int fordonsNr = Convert.ToInt32(Console.ReadLine());

            Fordon valtFordon = fordonLista.Find(f => f.Nummer == fordonsNr);
            if (valtFordon == null || !valtFordon.ArUthyrd)
            {
                Console.WriteLine("Fordonet hittades inte eller är inte uthyrt.");
                return;
            }

            Console.Write("Är kunden medlem? (ja/nej): ");
            bool arMedlem = Console.ReadLine().ToLower() == "ja";

            Console.Write("Är kunden personal? (ja/nej): ");
            bool arPersonal = Console.ReadLine().ToLower() == "ja";

            double grundPris = valtFordon.TimPris * valtFordon.AntalHyrtimmar;
            double rabattSumma = 0;
            string rabattText = "";

            // Rabattlogik: Personal får 50% rabatt, Medlemmar får 10% rabatt
            if (arPersonal)
            {
                rabattSumma = grundPris * 0.50;
                rabattText = "Personalrabatt (50 %)";
            }
            else if (arMedlem)
            {
                rabattSumma = grundPris * 0.10;
                rabattText = "Medlemsrabatt (10 %)";
            }

            double attBetala = grundPris - rabattSumma;

            // Fordonet blir ledigt igen
            valtFordon.ArUthyrd = false;

            // Skriv ut kvitto
            Console.WriteLine("\n--- KVITTO ---");
            Console.WriteLine($"{valtFordon.Typ}, {valtFordon.AntalHyrtimmar} timmar à {valtFordon.TimPris} kr\t{grundPris:F2} kr");
            if (rabattSumma > 0)
            {
                Console.WriteLine($"{rabattText}\t\t-{rabattSumma:F2} kr");
            }
            Console.WriteLine($"Att betala:\t\t\t\t{attBetala:F2} kr");
            Console.WriteLine("----------------");
        }

        // Laddar batteriet på en elcykel till 100%
        static void LaddaFordon(List<Fordon> fordonLista)
        {
            Console.Write("Fordonsnummer att ladda: ");
            int fordonsNr = Convert.ToInt32(Console.ReadLine());

            Fordon valtFordon = fordonLista.Find(f => f.Nummer == fordonsNr);
            if (valtFordon is Elcykel elcykel)
            {
                elcykel.BatteriNiva = 100;
                Console.WriteLine($"Elcykel {elcykel.Nummer} är nu fulladdad (100 %).");
            }
            else
            {
                Console.WriteLine("Detta fordon är en vanlig cykel eller finns inte, och saknar batteri.");
            }
        }
    }
}

      
