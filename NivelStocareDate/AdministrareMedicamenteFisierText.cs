using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareMedicamenteFisierText : IStocareData
    {
        private readonly string numeFisier;

        public AdministrareMedicamenteFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;

            Stream streamFisier = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisier.Close();
        }

        public void AddMedicament(Medicament medicament)
        {
            medicament.IdMedicament = GetNextId();

            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(medicament.ConversieLaSirPentruFisier());
            }
        }

        public List<Medicament> GetMedicamente()
        {
            List<Medicament> medicamente = new List<Medicament>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string? linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    medicamente.Add(new Medicament(linie));
                }
            }

            return medicamente;
        }

        public Medicament? GetMedicament(int id)
        {
            return GetMedicamente().FirstOrDefault(m => m.IdMedicament == id);
        }

        public List<Medicament> CautaDupaNume(string nume)
        {
            return GetMedicamente()
                .Where(m => m.nume.ToLower().Contains(nume.ToLower()))
                .ToList();
        }

        public List<Medicament> CautaDupaCategorie(CategorieMedicament categorie)
        {
            return GetMedicamente()
                .Where(m => m.categorie == categorie)
                .ToList();
        }

        public bool UpdateMedicament(Medicament medicamentActualizat)
        {
            List<Medicament> medicamente = GetMedicamente();
            bool modificat = false;

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var m in medicamente)
                {
                    Medicament deScris = m;

                    if (m.IdMedicament == medicamentActualizat.IdMedicament)
                    {
                        deScris = medicamentActualizat;
                        modificat = true;
                    }

                    sw.WriteLine(deScris.ConversieLaSirPentruFisier());
                }
            }

            return modificat;
        }

        public bool DeleteMedicament(int id)
        {
            List<Medicament> medicamente = GetMedicamente();
            var deSters = medicamente.FirstOrDefault(m => m.IdMedicament == id);

            if (deSters == null)
                return false;

            medicamente.Remove(deSters);

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var m in medicamente)
                {
                    sw.WriteLine(m.ConversieLaSirPentruFisier());
                }
            }

            return true;
        }

        private int GetNextId()
        {
            var medicamente = GetMedicamente();

            if (medicamente.Count == 0)
                return 1;

            return medicamente.Max(m => m.IdMedicament) + 1;
        }
    }
}