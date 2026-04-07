using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareProducatoriMemorie : IStocareProducatori
    {
        private List<Producator> producatori;

        public AdministrareProducatoriMemorie()
        {
            producatori = new List<Producator>();
        }

        public void AddProducator(Producator producator)
        {
            producator.IdProducator = GetNextId();
            producatori.Add(producator);
        }

        public List<Producator> GetProducatori()
        {
            return producatori;
        }

        public Producator? GetProducator(int id)
        {
            return producatori.FirstOrDefault(p => p.IdProducator == id);
        }

        public List<Producator> CautaDupaNume(string nume)
        {
            return producatori
                .Where(p => p.nume.ToLower().Contains(nume.ToLower()))
                .ToList();
        }

        public List<Producator> CautaDupaTara(TaraOrigine tara)
        {
            return producatori
                .Where(p => p.tara == tara)
                .ToList();
        }

        public bool UpdateProducator(Producator producatorActualizat)
        {
            var producatorGasit = producatori.FirstOrDefault(p => p.IdProducator == producatorActualizat.IdProducator);

            if (producatorGasit == null)
                return false;

            producatorGasit.nume = producatorActualizat.nume;
            producatorGasit.tara = producatorActualizat.tara;
            producatorGasit.certificari = producatorActualizat.certificari;

            return true;
        }

        public bool DeleteProducator(int id)
        {
            var producator = producatori.FirstOrDefault(p => p.IdProducator == id);

            if (producator == null)
                return false;

            producatori.Remove(producator);
            return true;
        }

        private int GetNextId()
        {
            if (producatori.Count == 0)
                return 1;

            return producatori.Max(p => p.IdProducator) + 1;
        }
    }
}
