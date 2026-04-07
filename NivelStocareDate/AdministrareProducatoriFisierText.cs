using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareProducatoriFisierText : IStocareProducatori
    {
        private readonly string numeFisier;

        public AdministrareProducatoriFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;

            Stream streamFisier = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisier.Close();
        }

        public void AddProducator(Producator producator)
        {
            producator.IdProducator = GetNextId();

            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(producator.ConversieLaSirPentruFisier());
            }
        }

        public List<Producator> GetProducatori()
        {
            List<Producator> producatori = new List<Producator>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string? linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    producatori.Add(new Producator(linie));
                }
            }

            return producatori;
        }

        public Producator? GetProducator(int id)
        {
            return GetProducatori().FirstOrDefault(p => p.IdProducator == id);
        }

        public List<Producator> CautaDupaNume(string nume)
        {
            return GetProducatori()
                .Where(p => p.nume.ToLower().Contains(nume.ToLower()))
                .ToList();
        }

        public List<Producator> CautaDupaTara(TaraOrigine tara)
        {
            return GetProducatori()
                .Where(p => p.tara == tara)
                .ToList();
        }

        public bool UpdateProducator(Producator producatorActualizat)
        {
            List<Producator> producatori = GetProducatori();
            bool modificat = false;

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var p in producatori)
                {
                    Producator deScris = p;

                    if (p.IdProducator == producatorActualizat.IdProducator)
                    {
                        deScris = producatorActualizat;
                        modificat = true;
                    }

                    sw.WriteLine(deScris.ConversieLaSirPentruFisier());
                }
            }

            return modificat;
        }

        public bool DeleteProducator(int id)
        {
            List<Producator> producatori = GetProducatori();
            var deSters = producatori.FirstOrDefault(p => p.IdProducator == id);

            if (deSters == null)
                return false;

            producatori.Remove(deSters);

            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (var p in producatori)
                {
                    sw.WriteLine(p.ConversieLaSirPentruFisier());
                }
            }

            return true;
        }

        private int GetNextId()
        {
            var producatori = GetProducatori();

            if (producatori.Count == 0)
                return 1;

            return producatori.Max(p => p.IdProducator) + 1;
        }
    }
}
