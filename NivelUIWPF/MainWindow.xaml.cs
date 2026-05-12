using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int NR_MAX_CARACTERE_NUME = 15;
        private const int NR_MAX_CARACTERE_PRODUCATOR = 15;

        private readonly IStocareData adminMedicamente;
        private Medicament medicamentCurent;

        public Medicament MedicamentCurent
        {
            get => medicamentCurent;
            set
            {
                medicamentCurent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            string caleFisier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Medicamente.txt");
            adminMedicamente = new AdministrareMedicamenteFisierText(caleFisier);

            cmbCategorie.ItemsSource = Enum.GetValues(typeof(CategorieMedicament));

            string[] formeMedicament = { "Tablete", "Capsule", "Sirop", "Unguent" };
            cmbFormaMedicament.ItemsSource = formeMedicament;

            MedicamentCurent = new Medicament();
            DataContext = this;

            cmbCategorie.SelectedIndex = 0;
            cmbFormaMedicament.SelectedIndex = 0;
            dtpDataExpirare.SelectedDate = DateTime.Today.AddYears(1);

            AfiseazaMedicamente();
        }

        private void AfiseazaMedicamente()
        {
            dgMedicamente.ItemsSource = null;
            dgMedicamente.ItemsSource = adminMedicamente.GetMedicamente();
        }

        private void btnMeniuAdministrare_Click(object sender, RoutedEventArgs e)
        {
            panelAdministrare.Visibility = Visibility.Visible;
            panelCautare.Visibility = Visibility.Collapsed;
        }

        private void btnMeniuCautare_Click(object sender, RoutedEventArgs e)
        {
            panelAdministrare.Visibility = Visibility.Collapsed;
            panelCautare.Visibility = Visibility.Visible;
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaErori();
            SincronizeazaDateDinInterfata();

            if (!ValideazaDateMedicament())
                return;

            adminMedicamente.AddMedicament(MedicamentCurent);

            MessageBox.Show("Medicamentul a fost adaugat cu succes.");

            AfiseazaMedicamente();
            ResetFormular();
        }

        private void btnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaErori();

            if (MedicamentCurent == null || MedicamentCurent.IdMedicament == 0)
            {
                MessageBox.Show("Selecteaza un medicament pentru actualizare.");
                return;
            }

            SincronizeazaDateDinInterfata();

            if (!ValideazaDateMedicament())
                return;

            bool succes = adminMedicamente.UpdateMedicament(MedicamentCurent);

            MessageBox.Show(succes
                ? "Medicamentul a fost actualizat cu succes."
                : "Actualizarea a esuat.");

            AfiseazaMedicamente();
            ResetFormular();
        }

        private void btnSterge_Click(object sender, RoutedEventArgs e)
        {
            if (MedicamentCurent == null || MedicamentCurent.IdMedicament == 0)
            {
                MessageBox.Show("Selecteaza un medicament pentru stergere.");
                return;
            }

            MessageBoxResult rezultat = MessageBox.Show(
                "Esti sigur ca vrei sa stergi medicamentul selectat?",
                "Confirmare stergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (rezultat != MessageBoxResult.Yes)
                return;

            bool succes = adminMedicamente.DeleteMedicament(MedicamentCurent.IdMedicament);

            MessageBox.Show(succes
                ? "Medicamentul a fost sters cu succes."
                : "Stergerea a esuat.");

            AfiseazaMedicamente();
            ResetFormular();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
        }

        private void dgMedicamente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMedicamente.SelectedItem is Medicament medicamentSelectat)
            {
                MedicamentCurent = medicamentSelectat;

                SeteazaModAdministrareInInterfata(MedicamentCurent.modAdministrare);

                btnActualizeaza.IsEnabled = true;
                btnSterge.IsEnabled = true;
            }
        }

        private void btnCauta_Click(object sender, RoutedEventArgs e)
        {
            string numeCautat = txtCautareNume.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeCautat))
            {
                txtRezultatCautare.Foreground = Brushes.Red;
                txtRezultatCautare.Text = "Introduce un nume pentru cautare.";
                return;
            }

            List<Medicament> rezultate = adminMedicamente.CautaDupaNume(numeCautat);

            if (rezultate.Count == 0)
            {
                txtRezultatCautare.Foreground = Brushes.Red;
                txtRezultatCautare.Text = "Nu s-a gasit niciun medicament.";
                return;
            }

            txtRezultatCautare.Foreground = Brushes.Black;
            txtRezultatCautare.Text = string.Join(
                Environment.NewLine + Environment.NewLine,
                rezultate.ConvertAll(m => m.Info()));
        }

        private void btnResetCautare_Click(object sender, RoutedEventArgs e)
        {
            txtCautareNume.Clear();
            txtRezultatCautare.Clear();
            txtRezultatCautare.Foreground = Brushes.Black;
        }

        private void SincronizeazaDateDinInterfata()
        {
            MedicamentCurent.nume = txtNume.Text.Trim();

            if (float.TryParse(txtPret.Text.Trim(), out float pret))
                MedicamentCurent.pret = pret;
            else
                MedicamentCurent.pret = 0;

            MedicamentCurent.producator = txtProducator.Text.Trim();

            if (cmbCategorie.SelectedItem is CategorieMedicament categorie)
                MedicamentCurent.categorie = categorie;

            MedicamentCurent.formaMedicament = cmbFormaMedicament.SelectedItem?.ToString() ?? string.Empty;
            MedicamentCurent.dataExpirare = dtpDataExpirare.SelectedDate ?? DateTime.Today;
            MedicamentCurent.modAdministrare = GetModAdministrareSelectat();
        }

        private ModAdministrare GetModAdministrareSelectat()
        {
            ModAdministrare modAdministrare = ModAdministrare.Niciuna;

            if (chkOral.IsChecked == true)
                modAdministrare |= ModAdministrare.Oral;

            if (chkInjectabil.IsChecked == true)
                modAdministrare |= ModAdministrare.Injectabil;

            if (chkTopic.IsChecked == true)
                modAdministrare |= ModAdministrare.Topic;

            if (chkInhalator.IsChecked == true)
                modAdministrare |= ModAdministrare.Inhalator;

            return modAdministrare;
        }

        private void SeteazaModAdministrareInInterfata(ModAdministrare modAdministrare)
        {
            chkOral.IsChecked = modAdministrare.HasFlag(ModAdministrare.Oral);
            chkInjectabil.IsChecked = modAdministrare.HasFlag(ModAdministrare.Injectabil);
            chkTopic.IsChecked = modAdministrare.HasFlag(ModAdministrare.Topic);
            chkInhalator.IsChecked = modAdministrare.HasFlag(ModAdministrare.Inhalator);
        }

        private void ResetFormular()
        {
            dgMedicamente.SelectedItem = null;

            MedicamentCurent = new Medicament();

            txtNume.Clear();
            txtPret.Clear();
            txtProducator.Clear();

            cmbCategorie.SelectedIndex = 0;
            cmbFormaMedicament.SelectedIndex = 0;
            dtpDataExpirare.SelectedDate = DateTime.Today.AddYears(1);

            chkOral.IsChecked = false;
            chkInjectabil.IsChecked = false;
            chkTopic.IsChecked = false;
            chkInhalator.IsChecked = false;

            btnActualizeaza.IsEnabled = false;
            btnSterge.IsEnabled = false;

            ReseteazaErori();
        }

        private bool ValideazaDateMedicament()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(MedicamentCurent.nume))
            {
                lblNume.Foreground = Brushes.Red;
                valid = false;
            }
            else if (MedicamentCurent.nume.Trim().Length > NR_MAX_CARACTERE_NUME)
            {
                lblNume.Foreground = Brushes.Red;
                valid = false;
            }

            if (MedicamentCurent.pret <= 0)
            {
                lblPret.Foreground = Brushes.Red;
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(MedicamentCurent.producator))
            {
                lblProducator.Foreground = Brushes.Red;
                valid = false;
            }
            else if (MedicamentCurent.producator.Trim().Length > NR_MAX_CARACTERE_PRODUCATOR)
            {
                lblProducator.Foreground = Brushes.Red;
                valid = false;
            }

            if (cmbCategorie.SelectedItem == null)
            {
                lblCategorie.Foreground = Brushes.Red;
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(MedicamentCurent.formaMedicament))
            {
                lblForma.Foreground = Brushes.Red;
                valid = false;
            }

            if (GetModAdministrareSelectat() == ModAdministrare.Niciuna)
            {
                lblModAdministrare.Foreground = Brushes.Red;
                valid = false;
            }

            if (!valid)
            {
                txtMesajEroare.Text = "Completeaza corect toate campurile marcate.";
                txtMesajEroare.Visibility = Visibility.Visible;
            }

            return valid;
        }

        private void ReseteazaErori()
        {
            lblNume.Foreground = Brushes.Black;
            lblPret.Foreground = Brushes.Black;
            lblProducator.Foreground = Brushes.Black;
            lblCategorie.Foreground = Brushes.Black;
            lblForma.Foreground = Brushes.Black;
            lblDataExpirare.Foreground = Brushes.Black;
            lblModAdministrare.Foreground = Brushes.Black;

            txtMesajEroare.Text = string.Empty;
            txtMesajEroare.Visibility = Visibility.Collapsed;
        }
    }
}