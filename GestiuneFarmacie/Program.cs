using System;
using GestiuneFarmacie.Models;
using GestiuneFarmacie.Services;

namespace GestiuneFarmacie
{
    internal class Program
    {
        private static void Main()
        {
            var farmacie = new Farmacie();
            Medicament? medicament = null;
            string optiune;

            do
            {
                Console.WriteLine("C. Citire medicament");
                Console.WriteLine("I. Afisare medicament");
                Console.WriteLine("X. Iesire");
                Console.Write("Alegeti o optiune: ");
                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "C":
                        medicament = CitireMedicament();
                        farmacie.AddMedicament(medicament);
                        break;
                    case "I":
                        farmacie.AfisareMedicament();
                        break;
                    case "X":
                        Console.WriteLine("Aplicatia se inchide.");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }

                Console.WriteLine();

            } while (optiune != "X");
        }

        private static Medicament CitireMedicament()
        {
            Console.Write("Introduceti numele medicamentului: ");
            var nume = Console.ReadLine() ?? string.Empty;

            Console.Write("Introduceti pretul: ");
            float.TryParse(Console.ReadLine(), out var pret);

            Console.Write("Introduceti cantitatea: ");
            int.TryParse(Console.ReadLine(), out var cantitate);

            return new Medicament(nume, pret, cantitate);
        }
    }
}
