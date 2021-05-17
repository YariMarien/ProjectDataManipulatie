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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (global.loginWindows.Visibility != Visibility.Visible)
            //{
            //global.loginWindows.Close();
            //}
            MainWindow m = new MainWindow();
            m.Show();
        }

        private void lblLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //global.loginWindows.Show();
            this.Close();
        }

        private void btnRegistreren_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
