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
    /// Interaction logic for ProfielWijzigen.xaml
    /// </summary>
    public partial class ProfielWijzigen : Window
    {
        public ProfielWijzigen()
        {
            InitializeComponent();
        }
        private void lblLogOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }
        private void lblProfiel_MouseDown()
        {

        }
        private void lblProfiel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            openProfile();
        }
        public void openProfile()
        {
            this.Hide();
            Profiel p = new Profiel();
            p.Show();
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Persoon user = DatabaseOperations.GetPersonById(global.currentUserId);
            txtEmail.Text = user.email;
            dprGeboorteDatum.SelectedDate = user.geboorteDatum;
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && dprGeboorteDatum.SelectedDate!=null)
            {
                this.Hide();
                DatabaseOperations.UpdatePerson(global.currentUserId, txtEmail.Text, (DateTime)dprGeboorteDatum.SelectedDate);
                MessageBox.Show("Je profiel is aangepast", "Gelukt", MessageBoxButton.OK);
                openProfile();
            }
            else
            {
                MessageBox.Show("Je moet een geldig email adres en geboortedatum ingeven.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
