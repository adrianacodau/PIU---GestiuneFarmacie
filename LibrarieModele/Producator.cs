using System;

namespace LibrarieModele
{
    public class Producator
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        public int IdProducator { get; set; }
        public string nume;
        public TaraOrigine tara;
        public Certificare certificari;

        public Producator()
        {
            IdProducator = 0;
            nume = string.Empty;
            tara = TaraOrigine.Alta;
            certificari = Certificare.Niciuna;
        }

        public Producator(int idProducator, string nume, TaraOrigine tara, Certificare certificari)
        {
            IdProducator = idProducator;
            this.nume = nume;
            this.tara = tara;
            this.certificari = certificari;
        }

        public Producator(string linieFisier)
        {
            var dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            IdProducator = Convert.ToInt32(dateFisier[0]);
            nume = dateFisier[1];
            tara = (TaraOrigine)Enum.Parse(typeof(TaraOrigine), dateFisier[2]);
            certificari = (Certificare)Enum.Parse(typeof(Certificare), dateFisier[3]);
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdProducator};{nume};{tara};{certificari}";
        }

        public string Info()
        {
            return $"Id: {IdProducator}, Nume: {nume}, Tara: {tara}, Certificari: {certificari}";
        }
    }
}
