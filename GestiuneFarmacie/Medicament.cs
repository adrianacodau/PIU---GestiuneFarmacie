namespace GestiuneFarmacie.Models
{
    internal class Medicament
    {
        public string Nume { get; set; }
        public float Pret { get; set; }
        public int Cantitate { get; set; }

        public Medicament(string nume, float pret, int cantitate)
        {
            Nume = nume;
            Pret = pret;
            Cantitate = cantitate;
        }

        public string Info()
        {
            return $"Nume: {Nume}, Pret: {Pret}, Cantitate: {Cantitate}";
        }
    }
}
