using System;

namespace LibrarieModele
{
    public class Medicament
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';

        public int IdMedicament { get; set; }
        public string nume;
        public float pret;
        public string producator;
        public CategorieMedicament categorie;
        public ModAdministrare modAdministrare;
        public string formaMedicament;
        public DateTime dataExpirare;

        //constructor implicit
        public Medicament()
        {
            IdMedicament = 0;
            nume = string.Empty;
            pret = 0;
            producator = string.Empty;
            categorie = CategorieMedicament.Altul;
            modAdministrare = ModAdministrare.Niciuna;

            formaMedicament = "Tablete";
            dataExpirare = DateTime.Today.AddYears(1);
        }
        //constructor cu parametri
        public Medicament(int idMedicament, string nume, float pret, string producator,
             CategorieMedicament categorie, ModAdministrare modAdministrare,
             string formaMedicament, DateTime dataExpirare)
                {
                    IdMedicament = idMedicament;
                    this.nume = nume;
                    this.pret = pret;
                    this.producator = producator;
                    this.categorie = categorie;
                    this.modAdministrare = modAdministrare;
                    this.formaMedicament = formaMedicament;
                    this.dataExpirare = dataExpirare;
                }

        public Medicament(string linieFisier)
        {
            var dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            IdMedicament = Convert.ToInt32(dateFisier[0]);
            nume = dateFisier[1];
            pret = Convert.ToSingle(dateFisier[2]);
            producator = dateFisier[3];
            categorie = (CategorieMedicament)Enum.Parse(typeof(CategorieMedicament), dateFisier[4]);
            modAdministrare = (ModAdministrare)Enum.Parse(typeof(ModAdministrare), dateFisier[5]);
            if (dateFisier.Length > 6)
            {
                formaMedicament = dateFisier[6];
            }
            else
            {
                formaMedicament = "Tablete";
            }

            if (dateFisier.Length > 7 && DateTime.TryParse(dateFisier[7], out DateTime data))
            {
                dataExpirare = data;
            }
            else
            {
                dataExpirare = DateTime.Today.AddYears(1);
            }
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdMedicament};{nume};{pret};{producator};{categorie};{modAdministrare};{formaMedicament};{dataExpirare:yyyy-MM-dd}";
        }

        public string Info()
        {
            return $"Id: {IdMedicament}, Nume: {nume}, Pret: {pret}, Producator: {producator}, Categorie: {categorie}, Mod administrare: {modAdministrare}, Forma: {formaMedicament}, Expira la: {dataExpirare:dd.MM.yyyy}";
        }
    }
}