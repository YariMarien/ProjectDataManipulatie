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
    /// Interaction logic for Profiel.xaml
    /// </summary>
    public partial class Profiel : Window
    {
        Persoon person = new Persoon();
        public Profiel()
        {
            InitializeComponent();
            person = DatabaseOperations.GetPersonById(global.currentUserId);
        }
        public Profiel(int personId)
        {
            InitializeComponent();
            person = DatabaseOperations.GetPersonById(personId);
        }
        private void lblLogOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (person.Id != global.currentUserId)
            {
                lblRelatieStatus.Visibility = Visibility.Visible;
                lblProfielWijzigen.Visibility = Visibility.Hidden;
            }
            else
            {
                lblRelatieStatus.Visibility = Visibility.Hidden;
                lblProfielWijzigen.Visibility = Visibility.Visible;
            }
            lblNaam.Content = "Naam: " + person.FullName;
            lblEmail.Content = "Email: " + person.email;
            lblGeboorteDatum.Content = "Geboortedatum: " + person.geboorteDatum.ToString("dd/MM/yyyy");
            lblClub.Content = "Club: " + person.CurrentClub.naam;

            //Check relation status
            if (person.Id != global.currentUserId)
            {
                var status = DatabaseOperations.GetRelationStatus(global.currentUserId, person.Id);
                switch (status)
                {
                    case ProjectDataManipulatie_DAL.Enums.RelatieStatus.Vrienden:
                        lblRelatieStatus.Content = "Vriend verwijderen";
                        break;
                    case ProjectDataManipulatie_DAL.Enums.RelatieStatus.VerzoekVerzonden:
                        lblRelatieStatus.Content = "Vriendschap verzoek verwijderen";
                        break;
                    case ProjectDataManipulatie_DAL.Enums.RelatieStatus.VerzoekOntvangen:
                        lblRelatieStatus.Content = "Vriend accepteren";
                        break;
                    case ProjectDataManipulatie_DAL.Enums.RelatieStatus.NietVerbonden:
                        lblRelatieStatus.Content = "Vriend toevoegen";
                        break;
                    default:
                        break;
                }
            }
        }
        private void lblProfielWijzigen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            ProfielWijzigen ProfielWijzigen = new ProfielWijzigen();
            ProfielWijzigen.Show();
            this.Close();
        }

        private void lblRelatieStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (lblRelatieStatus.Content)
            {
                case "Vriend toevoegen":
                    if (AddFriend())
                    {
                        DatabaseOperations.SendRelationShipRequest(global.currentUserId, person.Id);
                        MessageBox.Show("Vriendschapsverzoek is verzonden.", "Verzonden", MessageBoxButton.OK);
                        lblRelatieStatus.Content = "Vriendschap verzoek verwijderen";
                    }
                    else
                    {
                        MessageBox.Show("Vriendschapsverzoek is niet verzonden.", "Annulatie", MessageBoxButton.OK);
                    }
                    break;
                case "Vriendschap verzoek verwijderen":
                    if (CancelRelationRequest())
                    {
                        DatabaseOperations.CancelRelationShipRequest(global.currentUserId, person.Id);
                        MessageBox.Show("Vriendschapsverzoek is geannuleerd.", "Annulatie", MessageBoxButton.OK);
                        lblRelatieStatus.Content = "Vriend toevoegen";
                    }
                    else
                    {
                        MessageBox.Show("Het vriendschapsverzoek is niet geannuleerd.", "Toch behouden");
                    }
                    break;
                default:
                    break;
            }
        }
        public bool AddFriend()
        {
            var resp = MessageBox.Show("Zeker dat je " + person.FullName + " wil toevoegen als je vriend?", "Vriendschapsverzoek versturen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (resp)
            {
                case MessageBoxResult.None:
                    return AddFriend();
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return AddFriend();
            }
        }
        public bool CancelRelationRequest()
        {
            var resp = MessageBox.Show("Zeker dat je je veriendschapsverzoek met " + person.FullName + " wil annuleren?", "Vriendschapsverzoek annuleren", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (resp)
            {
                case MessageBoxResult.None:
                    return CancelRelationRequest();
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return CancelRelationRequest();
            }
        }
        private void lblPersonen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Personen p = new Personen();
            p.Show();
            this.Close();
        }

        private void lblProfiel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (person.Id != global.currentUserId)
            {
                person = DatabaseOperations.GetPersonById(global.currentUserId);
                Window_Loaded(sender, e);
            }
        }
    }
}
