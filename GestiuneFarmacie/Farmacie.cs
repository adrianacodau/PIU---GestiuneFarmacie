using System;
using GestiuneFarmacie.Models;

namespace GestiuneFarmacie.Services
{
    internal class Farmacie
    {
        public Medicament? MedicamentCurent { get; private set; }

        public void AddMedicament(Medicament m) => MedicamentCurent = m;

        public void AfisareMedicament()
        {
            if (MedicamentCurent is not null)
                Console.WriteLine(MedicamentCurent.Info());
            else
                Console.WriteLine("Nu exista niciun medicament adaugat.");
        }
    }
}
