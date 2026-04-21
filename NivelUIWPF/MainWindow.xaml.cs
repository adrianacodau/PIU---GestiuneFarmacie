using System;
using System.IO;
using System.Linq;
using System.Windows;
using LibrarieModele;
using NivelStocareDate;

namespace NivelUIWPF
{
    public partial class MainWindow : Window
    {
        private readonly IStocareData adminMedicamente;

        public MainWindow()
        {
            InitializeComponent();

            string radacinaSolutie = Directory
                .GetParent(AppDomain.CurrentDomain.BaseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .Parent!
                .FullName;

            string caleFisier = Path.Combine(
                radacinaSolutie,
                "GestiuneFarmacie",
                "bin",
                "Debug",
                "net8.0",
                "Medicamente.txt");

            MessageBox.Show(caleFisier);

            adminMedicamente = new AdministrareMedicamenteFisierText(caleFisier);
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
            txtCategorie.Text = medicament.categorie.ToString();
            txtModAdministrare.Text = medicament.modAdministrare.ToString();
        }
    }
}