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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectDataManipulatie_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string email, string password)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtEmail.Text = email;
            txtPassword.Password = password;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtEmail.Text = DatabaseOperations.GetPersonById(1).email;
            txtPassword.Password = DatabaseOperations.GetPersonById(1).password;
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseOperations.CheckLogin(txtEmail.Text, txtPassword.Password))
            {
                global.currentUserId = DatabaseOperations.GetPersonIdByEmail(txtEmail.Text);
            }
            this.Hide();
            Personen p = new Personen();
            p.Show();
            this.Close();
        }

        private void btnRegistreren_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Registreren r = new Registreren();
            r.Show();
            this.Close();
        }
    }
}
