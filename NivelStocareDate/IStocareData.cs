using System.Collections.Generic;
using LibrarieModele;

namespace NivelStocareDate
{
    public interface IStocareData
    {
        void AddMedicament(Medicament medicament);
        List<Medicament> GetMedicamente();
        Medicament? GetMedicament(int id);
        List<Medicament> CautaDupaNume(string nume);
        List<Medicament> CautaDupaCategorie(CategorieMedicament categorie);
        bool UpdateMedicament(Medicament medicamentActualizat);
        bool DeleteMedicament(int id);
    }
}