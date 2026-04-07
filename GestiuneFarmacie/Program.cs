using System;
using LibrarieModele;
using NivelStocareDate;

namespace GestiuneFarmacie
{
    internal class Program
    {
        private static void Main()
        {
            IStocareData adminMedicamente = new AdministrareMedicamenteFisierText("Medicamente.txt");
            IStocareProducatori adminProducatori = new AdministrareProducatoriFisierText("Producatori.txt");
            string optiune;
            

            do
            {
                Console.WriteLine("C. Citire medicament");
                Console.WriteLine("A. Afisare lista medicamente");
                Console.WriteLine("N. Cautare dupa nume");
                Console.WriteLine("G. Cautare dupa categorie");
                Console.WriteLine("P. Meniu producatori");
                Console.WriteLine("X. Iesire");
                Console.Write("Alegeti o optiune: ");
                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "C":
                        Medicament medicament = CitireMedicament();
                        adminMedicamente.AddMedicament(medicament);
                        Console.WriteLine("Medicament adaugat cu succes.");
                        break;

                    case "A":
                        var medicamente = adminMedicamente.GetMedicamente();
                        if (medicamente.Count == 0)
                            Console.WriteLine("Nu exista medicamente adaugate.");
                        else
                            foreach (var m in medicamente)
                                Console.WriteLine(m.Info());
                        break;

                    case "N":
                        Console.Write("Introduceti numele cautat: ");
                        string nume = Console.ReadLine() ?? string.Empty;

                        var rezultateNume = adminMedicamente.CautaDupaNume(nume);

                        if (rezultateNume.Count == 0)
                            Console.WriteLine("Nu s-au gasit medicamente.");
                        else
                            foreach (var m in rezultateNume)
                                Console.WriteLine(m.Info());
                        break;

                    case "G":
                        Console.WriteLine("Alegeti categoria:");
                        Console.WriteLine("1. Analgezic");
                        Console.WriteLine("2. Antibiotic");
                        Console.WriteLine("3. Antiinflamator");
                        Console.WriteLine("4. Vitamine");
                        Console.WriteLine("5. Antialergic");
                        Console.WriteLine("6. Altul");

                        int.TryParse(Console.ReadLine(), out int categorieInt);

                        if (Enum.IsDefined(typeof(CategorieMedicament), categorieInt))
                        {
                            var categorie = (CategorieMedicament)categorieInt;
                            var rezultate = adminMedicamente.CautaDupaCategorie(categorie);

                            if (rezultate.Count == 0)
                                Console.WriteLine("Nu s-au gasit medicamente.");
                            else
                                foreach (var m in rezultate)
                                    Console.WriteLine(m.Info());
                        }
                        else
                        {
                            Console.WriteLine("Categorie invalida.");
                        }

                        break;

                    case "P":
                        MeniuProducatori(adminProducatori);
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

            Console.Write("Introduceti producatorul: ");
            var producator = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Alegeti categoria:");
            Console.WriteLine("1. Analgezic");
            Console.WriteLine("2. Antibiotic");
            Console.WriteLine("3. Antiinflamator");
            Console.WriteLine("4. Vitamine");
            Console.WriteLine("5. Antialergic");
            Console.WriteLine("6. Altul");
            int.TryParse(Console.ReadLine(), out var categorieInt);

            CategorieMedicament categorie = Enum.IsDefined(typeof(CategorieMedicament), categorieInt)
                ? (CategorieMedicament)categorieInt
                : CategorieMedicament.Altul;

            ModAdministrare modAdministrare = ModAdministrare.Niciuna;

            Console.Write("Se administreaza oral? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                modAdministrare |= ModAdministrare.Oral;

            Console.Write("Se administreaza injectabil? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                modAdministrare |= ModAdministrare.Injectabil;

            Console.Write("Se administreaza topic? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                modAdministrare |= ModAdministrare.Topic;

            Console.Write("Se administreaza inhalator? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                modAdministrare |= ModAdministrare.Inhalator;

            return new Medicament(0, nume, pret, producator, categorie, modAdministrare);
        }
        private static void MeniuProducatori(IStocareProducatori adminProducatori)
        {
            string optiune;

            do
            {
                Console.WriteLine();
                Console.WriteLine("=== MENIU PRODUCATORI ===");
                Console.WriteLine("C. Citire producator");
                Console.WriteLine("A. Afisare lista producatori");
                Console.WriteLine("N. Cautare dupa nume");
                Console.WriteLine("T. Cautare dupa tara");
                Console.WriteLine("X. Inapoi");
                Console.Write("Alegeti o optiune: ");

                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "C":
                        Producator producator = CitireProducator();
                        adminProducatori.AddProducator(producator);
                        Console.WriteLine("Producator adaugat cu succes.");
                        break;

                    case "A":
                        var producatori = adminProducatori.GetProducatori();
                        if (producatori.Count == 0)
                            Console.WriteLine("Nu exista producatori.");
                        else
                            foreach (var p in producatori)
                                Console.WriteLine(p.Info());
                        break;

                    case "N":
                        Console.Write("Introduceti numele cautat: ");
                        string nume = Console.ReadLine() ?? string.Empty;
                        var rezultateNume = adminProducatori.CautaDupaNume(nume);

                        if (rezultateNume.Count == 0)
                            Console.WriteLine("Nu s-au gasit producatori.");
                        else
                            foreach (var p in rezultateNume)
                                Console.WriteLine(p.Info());
                        break;

                    case "T":
                        AfiseazaTari();
                        int.TryParse(Console.ReadLine(), out int taraInt);

                        if (Enum.IsDefined(typeof(TaraOrigine), taraInt))
                        {
                            var tara = (TaraOrigine)taraInt;
                            var rezultate = adminProducatori.CautaDupaTara(tara);

                            if (rezultate.Count == 0)
                                Console.WriteLine("Nu s-au gasit producatori.");
                            else
                                foreach (var p in rezultate)
                                    Console.WriteLine(p.Info());
                        }
                        else
                        {
                            Console.WriteLine("Tara invalida.");
                        }
                        break;

                    case "X":
                        Console.WriteLine("Revenire la meniul principal.");
                        break;

                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }

                Console.WriteLine();

            } while (optiune != "X");
        }
        private static Producator CitireProducator()
        {
            Console.Write("Introduceti numele producatorului: ");
            string nume = Console.ReadLine() ?? string.Empty;

            AfiseazaTari();
            int.TryParse(Console.ReadLine(), out int taraInt);

            TaraOrigine tara = Enum.IsDefined(typeof(TaraOrigine), taraInt)
                ? (TaraOrigine)taraInt
                : TaraOrigine.Alta;

            Certificare certificari = Certificare.Niciuna;

            Console.Write("Are GMP? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                certificari |= Certificare.GMP;

            Console.Write("Are ISO? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                certificari |= Certificare.ISO;

            Console.Write("Are Bio? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                certificari |= Certificare.Bio;

            Console.Write("Are certificare UE? (da/nu): ");
            if ((Console.ReadLine() ?? "").ToLower() == "da")
                certificari |= Certificare.UE;

            return new Producator(0, nume, tara, certificari);
        }
        private static void AfiseazaTari()
        {
            Console.WriteLine("Alegeti tara:");
            Console.WriteLine("1. Romania");
            Console.WriteLine("2. Germania");
            Console.WriteLine("3. Franta");
            Console.WriteLine("4. Italia");
            Console.WriteLine("5. SUA");
            Console.WriteLine("6. Alta");
        }
    }
}
