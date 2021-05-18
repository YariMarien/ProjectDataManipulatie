using ProjectDataManipulatie_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectDataManipulatie_WPF
{
    /// <summary>
    /// Interaction logic for Registreren.xaml
    /// </summary>
    public partial class Registreren : Window
    {
        public Registreren()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void btnRegistreren_Click(object sender, RoutedEventArgs e)
        {
            Persoon p = new Persoon()
            {
                naam = txtName.Text,
                voornaam = txtFirstName.Text,
                password = txtPassword.Password,
                email = txtEmail.Text,
                geboorteDatum = (DateTime)dpGeboortedatum.SelectedDate,
                geslacht = GetSelectedGeslacht()
            };
            if (DatabaseOperations.CreatePerson(p))
            {
                MessageBox.Show("Je account is aangemaakt.", "Gelukt", MessageBoxButton.OK);
                this.Hide();
                MainWindow m = new MainWindow(txtEmail.Text, txtPassword.Password);
                m.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Er bestaat al een account met dit mail adres.", "Niet gelukt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public string GetSelectedGeslacht()
        {
            if ((bool)rdbMan.IsChecked)
            {
                return "Man";
            }
            else
            {
                return "Vrouw";
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            this.Hide();
            m.Show();
            this.Close();
        }
    }
}
