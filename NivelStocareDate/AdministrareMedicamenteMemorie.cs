using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareMedicamenteMemorie : IStocareData
    {
        private List<Medicament> medicamente;

        public AdministrareMedicamenteMemorie()
        {
            medicamente = new List<Medicament>();
        }

        public void AddMedicament(Medicament medicament)
        {
            medicament.IdMedicament = GetNextId();
            medicamente.Add(medicament);
        }

        public List<Medicament> GetMedicamente()
        {
            return medicamente;
        }

        public Medicament? GetMedicament(int id)
        {
            return medicamente.FirstOrDefault(m => m.IdMedicament == id);
        }

        public List<Medicament> CautaDupaNume(string nume)
        {
            return medicamente
                .Where(m => m.nume.ToLower().Contains(nume.ToLower()))
                .ToList();
        }

        public List<Medicament> CautaDupaCategorie(CategorieMedicament categorie)
        {
            return medicamente
                .Where(m => m.categorie == categorie)
                .ToList();
        }

        public bool UpdateMedicament(Medicament medicamentActualizat)
        {
            var medicamentGasit = medicamente.FirstOrDefault(m => m.IdMedicament == medicamentActualizat.IdMedicament);

            if (medicamentGasit == null)
            {
                return false;
            }

            medicamentGasit.nume = medicamentActualizat.nume;
            medicamentGasit.pret = medicamentActualizat.pret;
            medicamentGasit.producator = medicamentActualizat.producator;
            medicamentGasit.categorie = medicamentActualizat.categorie;
            medicamentGasit.modAdministrare = medicamentActualizat.modAdministrare;

            return true;
        }

        public bool DeleteMedicament(int id)
        {
            var medicament = medicamente.FirstOrDefault(m => m.IdMedicament == id);

            if (medicament == null)
            {
                return false;
            }

            medicamente.Remove(medicament);
            return true;
        }

        private int GetNextId()
        {
            if (medicamente.Count == 0)
            {
                return 1;
            }

            return medicamente.Max(m => m.IdMedicament) + 1;
        }
    }
}