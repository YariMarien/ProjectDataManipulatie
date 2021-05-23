using ProjectDataManipulatie_DAL;
using dto = ProjectDataManipulatie_DAL.dto;
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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            person = DatabaseOperations.GetPersonById((int)(int)(int)global.currentUserId);
        }
        public Profiel(int personId)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            person = DatabaseOperations.GetPersonById(personId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckIfOwnProfile();

            SetPersonData();

            
            if (person.Id != (int)(int)global.currentUserId)
            {
                CheckRelationStatus();
            }
            else
            {
                SetFriends();
                GetRelationRequests();
            }
        }

        public void GetRelationRequests()
        {
            List<Relatie> Requests = DatabaseOperations.GetUnacceptedRelationRequests((int)(int)global.currentUserId);
            List<Persoon> SendRequests = new List<Persoon>();
            List<Persoon> ReceivedRequests = new List<Persoon>();
            foreach (var request in Requests)
            {
                if (DatabaseOperations.GetRelationRequestSenderByRelationId(request.Id).Id == (int)(int)global.currentUserId)
                {
                    SendRequests.Add(DatabaseOperations.GetRelationRequestReceiverByRelationId(request.Id));
                }
                else
                {
                    ReceivedRequests.Add(DatabaseOperations.GetRelationRequestSenderByRelationId(request.Id));
                }
            }

            if (SendRequests.Count > 0)
            {
                lbxVerzondenVriendschapsverzoeken.ItemsSource = SendRequests;
                lbxVerzondenVriendschapsverzoeken.SelectedValuePath = "Id";
                lbxVerzondenVriendschapsverzoeken.DisplayMemberPath = "FullName";
            }
            else
            {
                lbxVerzondenVriendschapsverzoeken.ItemsSource = new List<Persoon> { new Persoon() { naam = "Geen niet geaccepteerde veriendschapsverzoeken verzonden." } };
                lbxVerzondenVriendschapsverzoeken.IsEnabled = false;
                lbxVerzondenVriendschapsverzoeken.SelectedValuePath = "Id";
                lbxVerzondenVriendschapsverzoeken.DisplayMemberPath = "FullName";
            }
            if (ReceivedRequests.Count > 0)
            {
                lbxOntvangenVriendschapsverzoeken.ItemsSource = ReceivedRequests;
                lbxOntvangenVriendschapsverzoeken.SelectedValuePath = "Id";
                lbxOntvangenVriendschapsverzoeken.DisplayMemberPath = "FullName";
            }
            else
            {
                lbxOntvangenVriendschapsverzoeken.ItemsSource = new List<Persoon> { new Persoon() { naam = "Geen niet geaccepteerde veriendschapsverzoeken ontvangen." } };
                lbxOntvangenVriendschapsverzoeken.IsEnabled = false;
                lbxOntvangenVriendschapsverzoeken.SelectedValuePath = "Id";
                lbxOntvangenVriendschapsverzoeken.DisplayMemberPath = "FullName";
            }
        }

        public void CheckRelationStatus()
        {
            var status = DatabaseOperations.GetRelationStatus((int)(int)global.currentUserId, person.Id);
            switch (status)
            {
                case ProjectDataManipulatie_DAL.Enums.RelatieStatus.Vrienden:
                    btnRelatieStatus.Content = "Vriend verwijderen";
                    break;
                case ProjectDataManipulatie_DAL.Enums.RelatieStatus.VerzoekVerzonden:
                    btnRelatieStatus.Content = "Vriendschap verzoek verwijderen";
                    break;
                case ProjectDataManipulatie_DAL.Enums.RelatieStatus.VerzoekOntvangen:
                    btnRelatieStatus.Content = "Vriend accepteren";
                    break;
                case ProjectDataManipulatie_DAL.Enums.RelatieStatus.NietVerbonden:
                    btnRelatieStatus.Content = "Vriend toevoegen";
                    break;
                default:
                    break;
            }
        }

        public void CheckIfOwnProfile()
        {
            if (person.Id != (int)(int)global.currentUserId)
            {
                btnRelatieStatus.Visibility = Visibility.Visible;
                btnProfielWijzigen.Visibility = Visibility.Hidden;
                tbcRelatieVerzoeken.Visibility = Visibility.Hidden;
                lblVerzoeken.Visibility = Visibility.Hidden;
                colLoggedIn.Width = new GridLength(0);
            }
            else
            {
                btnRelatieStatus.Visibility = Visibility.Hidden;
                btnProfielWijzigen.Visibility = Visibility.Visible;
                tbcRelatieVerzoeken.Visibility = Visibility.Visible;
                lblVerzoeken.Visibility = Visibility.Visible;
                colLoggedIn.Width = new GridLength(200);
            }
        }

        public void SetPersonData()
        {
            lblNaam.Content = "Naam: " + person.FullName;
            lblEmail.Content = "Email: " + person.email;
            lblGeboorteDatum.Content = "Geboortedatum: " + person.geboorteDatum.ToString("dd/MM/yyyy");
            if (person.PersonenClubs.Count>0)
            {
                lblClub.Content = "Club: " + person.CurrentClub.naam;
            }
            else
            {
                lblClub.Content = "Deze persoon is geen atleet.";
            }
            
        }

        public void SetFriends() {
            dgVrienden.ItemsSource = DatabaseOperations.GetFriends((int)(int)global.currentUserId);
        }

        private void lblRelatieStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
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
        public bool AcceptRelationRequest()
        {
            var resp = MessageBox.Show("Zeker dat je je veriendschapsverzoek van " + person.FullName + " wil accepteren?", "Vriendschapsverzoek accepteren", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (resp)
            {
                case MessageBoxResult.None:
                    return AcceptRelationRequest();
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return AcceptRelationRequest();
            }
        }
        public bool DeleteFriend()
        {
            var resp = MessageBox.Show("Zeker " + person.FullName + " uit je vrienden wil verwijderen?", "Vriend verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (resp)
            {
                case MessageBoxResult.None:
                    return DeleteFriend();
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return DeleteFriend();
            }
        }

        private void lbxOntvangenVriendschapsverzoeken_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxOntvangenVriendschapsverzoeken.SelectedItem != null)
            {
                person = DatabaseOperations.GetPersonById((lbxOntvangenVriendschapsverzoeken.SelectedItem as Persoon).Id);
                Window_Loaded(sender, e);
            }

        }

        private void lbxVerzondenVriendschapsverzoeken_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxVerzondenVriendschapsverzoeken.SelectedItem != null)
            {
                person = DatabaseOperations.GetPersonById((lbxVerzondenVriendschapsverzoeken.SelectedItem as Persoon).Id);
                Window_Loaded(sender, e);
            }

        }

        private void lblGegevens_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Naviagte(0);
        }

        private void lblVrienden_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Naviagte(1);
        }

        private void lblRecords_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Naviagte(2);
        }

        public void Naviagte(int nmbr)
        {
            spGegevens.Visibility = Visibility.Collapsed;
            dgVrienden.Visibility = Visibility.Collapsed;
            switch (nmbr)
            {
                case 0:
                    spGegevens.Visibility = Visibility.Visible;
                    break;
                case 1:
                    dgVrienden.Visibility = Visibility.Visible;
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        private void dgVrienden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVrienden.SelectedItem!=null)
            {
                person = DatabaseOperations.GetPersonById((dgVrienden.SelectedItem as Persoon).Id);
                Window_Loaded(sender, e);
                Naviagte(0);
            }
        }

        private void btnPersonen_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Personen p = new Personen();
            p.Show();
            this.Close();
        }

        private void btnProfiel_Click(object sender, RoutedEventArgs e)
        {
            if (person.Id != (int)(int)global.currentUserId)
            {
                person = DatabaseOperations.GetPersonById((int)(int)global.currentUserId);
                Window_Loaded(sender, e);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void btnRelatieStatus_Click(object sender, RoutedEventArgs e)
        {
            switch (btnRelatieStatus.Content)
            {
                case "Vriend toevoegen":
                    if (AddFriend())
                    {
                        DatabaseOperations.SendRelationRequest((int)(int)global.currentUserId, person.Id);
                        MessageBox.Show("Vriendschapsverzoek is verzonden.", "Verzonden", MessageBoxButton.OK);
                        btnRelatieStatus.Content = "Vriendschap verzoek verwijderen";
                    }
                    else
                    {
                        MessageBox.Show("Vriendschapsverzoek is niet verzonden.", "Annulatie", MessageBoxButton.OK);
                    }
                    break;
                case "Vriendschap verzoek verwijderen":
                    if (CancelRelationRequest())
                    {
                        DatabaseOperations.CancelRelationRequest((int)(int)global.currentUserId, person.Id);
                        MessageBox.Show("Vriendschapsverzoek is geannuleerd.", "Annulatie", MessageBoxButton.OK);
                        btnRelatieStatus.Content = "Vriend toevoegen";
                    }
                    else
                    {
                        MessageBox.Show("Het vriendschapsverzoek is niet geannuleerd.", "Toch behouden");
                    }
                    break;
                case "Vriend accepteren":
                    if (AcceptRelationRequest())
                    {
                        DatabaseOperations.AcceptRelationRequest(person.Id, (int)(int)global.currentUserId);
                        MessageBox.Show("Vriendschapsverzoek is geaccepteerd.", "Gelukt", MessageBoxButton.OK);
                        btnRelatieStatus.Content = "Vriend verwijderen";
                    }
                    else
                    {
                        MessageBox.Show("Het vriendschapsverzoek is niet geaccepteerd.", "Toch niet toevoegen");
                    }
                    break;
                case "Vriend verwijderen":
                    if (DeleteFriend())
                    {
                        DatabaseOperations.DeleteFriend(person.Id, (int)(int)global.currentUserId);
                        MessageBox.Show(person.FullName + "is uit je vrienden verwijderd.", "Vriend verwijderd", MessageBoxButton.OK);
                        btnRelatieStatus.Content = "Vriend toevoegen";
                    }
                    else
                    {
                        MessageBox.Show(person.FullName + " is niet verwijderd als vriend.", "Toch niet verwijderen");
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnProfielWijzigen_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ProfielWijzigen ProfielWijzigen = new ProfielWijzigen();
            ProfielWijzigen.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
