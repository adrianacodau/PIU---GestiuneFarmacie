using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LibrarieModele;
using NivelStocareDate;

namespace NivelUIWPF
{
    [Flags]
    public enum EroriValidareMedicament
    {
        None = 0,
        NumeGol = 1,
        NumePreaLung = 2,
        PretGol = 4,
        PretInvalid = 8,
        ProducatorGol = 16,
        ProducatorPreaLung = 32,
        CategorieInvalida = 64,
        RetetaNespecificata = 128,
        ModAdministrareLipsa = 256
    }

    public partial class MainWindow : Window
    {
        private const int NR_MAX_CARACTERE_NUME = 15;
        private const int NR_MAX_CARACTERE_PRODUCATOR = 15;

        private readonly IStocareData adminMedicamente;

        public MainWindow()
        {
            InitializeComponent();

            string caleFisier = @"C:\Users\adriana2\Desktop\PIU\GestiuneFarmacie\GestiuneFarmacie\bin\Debug\net8.0\Medicamente.txt";
            adminMedicamente = new AdministrareMedicamenteFisierText(caleFisier);

            cmbCategorie.ItemsSource = Enum.GetValues(typeof(CategorieMedicament));

            AfiseazaPanelAdministrare();
            ResetFormular();
            ResetCautare();
        }

        private void btnMeniuAdministrare_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanelAdministrare();
        }

        private void btnMeniuCautare_Click(object sender, RoutedEventArgs e)
        {
            AfiseazaPanelCautare();
        }

        private void AfiseazaPanelAdministrare()
        {
            panelAdministrare.Visibility = Visibility.Visible;
            panelCautare.Visibility = Visibility.Collapsed;
            txtMesajEroare.Visibility = Visibility.Collapsed;
        }

        private void AfiseazaPanelCautare()
        {
            panelAdministrare.Visibility = Visibility.Collapsed;
            panelCautare.Visibility = Visibility.Visible;
            txtMesajEroare.Visibility = Visibility.Collapsed;
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaErori();

            var codValidare = ValideazaDateMedicament();

            if (codValidare != EroriValidareMedicament.None)
            {
                MarcheazaErori(codValidare);
                AfiseazaMesajEroare(codValidare);
                return;
            }

            ModAdministrare modAdministrare = ModAdministrare.Niciuna;
            if (chkOral.IsChecked == true) modAdministrare |= ModAdministrare.Oral;
            if (chkInjectabil.IsChecked == true) modAdministrare |= ModAdministrare.Injectabil;
            if (chkTopic.IsChecked == true) modAdministrare |= ModAdministrare.Topic;
            if (chkInhalator.IsChecked == true) modAdministrare |= ModAdministrare.Inhalator;

            Medicament medicamentNou = new Medicament(
                0,
                txtNume.Text.Trim(),
                float.Parse(txtPret.Text.Trim()),
                txtProducator.Text.Trim(),
                (CategorieMedicament)cmbCategorie.SelectedItem,
                modAdministrare
            );

            adminMedicamente.AddMedicament(medicamentNou);

            MessageBox.Show("Medicamentul a fost adăugat cu succes în fișier.");
            ResetFormular();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
        }

        private void btnIncarca_Click(object sender, RoutedEventArgs e)
        {
            var medicament = adminMedicamente.GetMedicamente().FirstOrDefault();

            if (medicament == null)
            {
                MessageBox.Show("Nu există medicamente în fișier.");
                return;
            }

            txtNume.Text = medicament.nume;
            txtPret.Text = medicament.pret.ToString();
            txtProducator.Text = medicament.producator;
            cmbCategorie.SelectedItem = medicament.categorie;

            rbRetetaNu.IsChecked = true;

            chkOral.IsChecked = medicament.modAdministrare.HasFlag(ModAdministrare.Oral);
            chkInjectabil.IsChecked = medicament.modAdministrare.HasFlag(ModAdministrare.Injectabil);
            chkTopic.IsChecked = medicament.modAdministrare.HasFlag(ModAdministrare.Topic);
            chkInhalator.IsChecked = medicament.modAdministrare.HasFlag(ModAdministrare.Inhalator);
        }

        private void btnCauta_Click(object sender, RoutedEventArgs e)
        {
            string numeCautat = txtCautareNume.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeCautat))
            {
                txtRezultatCautare.Text = "Introduceți un nume pentru căutare.";
                txtRezultatCautare.Foreground = Brushes.Red;
                return;
            }

            var rezultate = adminMedicamente.CautaDupaNume(numeCautat);

            if (rezultate == null || rezultate.Count == 0)
            {
                txtRezultatCautare.Text = "Nu s-a găsit niciun medicament cu numele introdus.";
                txtRezultatCautare.Foreground = Brushes.Red;
                return;
            }

            txtRezultatCautare.Foreground = Brushes.Black;
            txtRezultatCautare.Text = string.Join(Environment.NewLine + Environment.NewLine,
                rezultate.Select(m =>
                    $"Id: {m.IdMedicament}\n" +
                    $"Nume: {m.nume}\n" +
                    $"Preț: {m.pret}\n" +
                    $"Producător: {m.producator}\n" +
                    $"Categorie: {m.categorie}\n" +
                    $"Mod administrare: {m.modAdministrare}"));
        }

        private void btnResetCautare_Click(object sender, RoutedEventArgs e)
        {
            ResetCautare();
        }

        private EroriValidareMedicament ValideazaDateMedicament()
        {
            EroriValidareMedicament rezultat = EroriValidareMedicament.None;

            if (string.IsNullOrWhiteSpace(txtNume.Text))
                rezultat |= EroriValidareMedicament.NumeGol;
            else if (txtNume.Text.Trim().Length > NR_MAX_CARACTERE_NUME)
                rezultat |= EroriValidareMedicament.NumePreaLung;

            if (string.IsNullOrWhiteSpace(txtPret.Text))
                rezultat |= EroriValidareMedicament.PretGol;
            else if (!float.TryParse(txtPret.Text.Trim(), out float pret) || pret <= 0)
                rezultat |= EroriValidareMedicament.PretInvalid;

            if (string.IsNullOrWhiteSpace(txtProducator.Text))
                rezultat |= EroriValidareMedicament.ProducatorGol;
            else if (txtProducator.Text.Trim().Length > NR_MAX_CARACTERE_PRODUCATOR)
                rezultat |= EroriValidareMedicament.ProducatorPreaLung;

            if (cmbCategorie.SelectedItem == null)
                rezultat |= EroriValidareMedicament.CategorieInvalida;

            if (rbRetetaDa.IsChecked != true && rbRetetaNu.IsChecked != true)
                rezultat |= EroriValidareMedicament.RetetaNespecificata;

            bool existaModAdministrare =
                chkOral.IsChecked == true ||
                chkInjectabil.IsChecked == true ||
                chkTopic.IsChecked == true ||
                chkInhalator.IsChecked == true;

            if (!existaModAdministrare)
                rezultat |= EroriValidareMedicament.ModAdministrareLipsa;

            return rezultat;
        }

        private void MarcheazaErori(EroriValidareMedicament erori)
        {
            if (erori.HasFlag(EroriValidareMedicament.NumeGol) ||
                erori.HasFlag(EroriValidareMedicament.NumePreaLung))
                lblNume.Foreground = Brushes.Red;

            if (erori.HasFlag(EroriValidareMedicament.PretGol) ||
                erori.HasFlag(EroriValidareMedicament.PretInvalid))
                lblPret.Foreground = Brushes.Red;

            if (erori.HasFlag(EroriValidareMedicament.ProducatorGol) ||
                erori.HasFlag(EroriValidareMedicament.ProducatorPreaLung))
                lblProducator.Foreground = Brushes.Red;

            if (erori.HasFlag(EroriValidareMedicament.CategorieInvalida))
                lblCategorie.Foreground = Brushes.Red;

            if (erori.HasFlag(EroriValidareMedicament.RetetaNespecificata))
                lblReteta.Foreground = Brushes.Red;

            if (erori.HasFlag(EroriValidareMedicament.ModAdministrareLipsa))
                lblModAdministrare.Foreground = Brushes.Red;
        }

        private void AfiseazaMesajEroare(EroriValidareMedicament erori)
        {
            string mesaj = "";

            if (erori.HasFlag(EroriValidareMedicament.NumeGol))
                mesaj += "- Numele trebuie completat.\n";

            if (erori.HasFlag(EroriValidareMedicament.NumePreaLung))
                mesaj += $"- Numele nu poate avea mai mult de {NR_MAX_CARACTERE_NUME} caractere.\n";

            if (erori.HasFlag(EroriValidareMedicament.PretGol))
                mesaj += "- Prețul trebuie completat.\n";

            if (erori.HasFlag(EroriValidareMedicament.PretInvalid))
                mesaj += "- Prețul trebuie să fie numeric și mai mare decât 0.\n";

            if (erori.HasFlag(EroriValidareMedicament.ProducatorGol))
                mesaj += "- Producătorul trebuie completat.\n";

            if (erori.HasFlag(EroriValidareMedicament.ProducatorPreaLung))
                mesaj += $"- Producătorul nu poate avea mai mult de {NR_MAX_CARACTERE_PRODUCATOR} caractere.\n";

            if (erori.HasFlag(EroriValidareMedicament.CategorieInvalida))
                mesaj += "- Categoria trebuie selectată.\n";

            if (erori.HasFlag(EroriValidareMedicament.RetetaNespecificata))
                mesaj += "- Selectați dacă medicamentul necesită rețetă.\n";

            if (erori.HasFlag(EroriValidareMedicament.ModAdministrareLipsa))
                mesaj += "- Selectați cel puțin un mod de administrare.\n";

            txtMesajEroare.Text = mesaj;
            txtMesajEroare.Visibility = Visibility.Visible;
        }

        private void ResetFormular()
        {
            txtNume.Text = string.Empty;
            txtPret.Text = string.Empty;
            txtProducator.Text = string.Empty;
            cmbCategorie.SelectedItem = null;

            rbRetetaDa.IsChecked = false;
            rbRetetaNu.IsChecked = false;

            chkOral.IsChecked = false;
            chkInjectabil.IsChecked = false;
            chkTopic.IsChecked = false;
            chkInhalator.IsChecked = false;

            ReseteazaErori();
        }

        private void ResetCautare()
        {
            txtCautareNume.Text = string.Empty;
            txtRezultatCautare.Text = string.Empty;
            txtRezultatCautare.Foreground = Brushes.Black;
        }

        private void ReseteazaErori()
        {
            lblNume.Foreground = Brushes.Black;
            lblPret.Foreground = Brushes.Black;
            lblProducator.Foreground = Brushes.Black;
            lblCategorie.Foreground = Brushes.Black;
            lblReteta.Foreground = Brushes.Black;
            lblModAdministrare.Foreground = Brushes.Black;

            txtMesajEroare.Text = string.Empty;
            txtMesajEroare.Visibility = Visibility.Collapsed;
        }
    }
}