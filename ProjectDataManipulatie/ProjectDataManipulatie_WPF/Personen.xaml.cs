//using ProjectDataManipulatie_DAL;
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
    /// Interaction logic for Personen.xaml
    /// </summary>
    public partial class Personen : Window
    {
        public Personen()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get people
            var a = DatabaseOperations.SearchPerson();
            //var b = a[0].PersonenClubs[0];
            dgPersonen.ItemsSource = a;

            //Get provinces
            cmbProvincie.ItemsSource = DatabaseOperations.GetAllProvinces();
            cmbProvincie.DisplayMemberPath = "naam";
            cmbProvincie.SelectedValuePath = "id";

            //Get clubs
            cmbClub.ItemsSource = DatabaseOperations.GetAllClubs();
            cmbClub.DisplayMemberPath = "naam";
            cmbClub.SelectedValuePath = "id";
        }

        private void txtZoeken_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        public void Search()
        {
            bool? vlaandere = null;
            bool? wallonie = null;
            if (cbVlaanderen != null && cbWallonië != null)
            {
                vlaandere = cbVlaanderen.IsChecked;
                wallonie = cbWallonië.IsChecked;
                var zoekData = txtZoeken.Text;
                int? provincieId = null;
                if (cmbProvincie != null && cmbProvincie.SelectedItem != null)
                {
                    provincieId = (cmbProvincie.SelectedItem as Provincie).Id;
                }
                int? clubId = null;
                if (cmbClub != null && cmbClub.SelectedItem != null)
                {
                    clubId = (cmbClub.SelectedItem as Club).Id;
                }
                var PersonenLijst = dgPersonen.ItemsSource = DatabaseOperations.SearchPerson(vlaandere, wallonie, zoekData, provincieId, clubId);
                dgPersonen.ItemsSource = PersonenLijst;
            }
        }
        public void FillClubs()
        {
            if (cbVlaanderen != null && cbWallonië != null && cmbClub != null && cmbProvincie!=null)
            {
                int? current = null;
                if (cmbClub.SelectedItem != null)
                {
                    current = (cmbClub.SelectedItem as Club).Id;
                }
                int? provinceId = null;
                if (cmbProvincie.SelectedItem != null)
                {
                    provinceId = (cmbProvincie.SelectedItem as Provincie).Id;
                    var clubs = DatabaseOperations.GetClubsByProvince(provinceId);
                    cmbClub.ItemsSource = clubs;
                    for (int i = 0; i < clubs.Count; i++)
                    {
                        if (clubs[i].Id == current)
                        {
                            cmbClub.SelectedIndex = i;
                        }
                    }
                }
                else
                {
                    bool? vlaandere = null;
                    bool? wallonie = null;
                    if (cbVlaanderen != null)
                    {
                        vlaandere = cbVlaanderen.IsChecked;
                    }
                    if (cbWallonië != null)
                    {
                        wallonie = cbWallonië.IsChecked;
                    }
                    cmbClub.ItemsSource = DatabaseOperations.GetClubsByRegion(vlaandere, wallonie);
                }
            }
        }
        public void FillProvince()
        {
            if (cbVlaanderen != null && cbWallonië != null && cmbProvincie != null)
            {
                int? current = null;
                if (cmbProvincie.SelectedItem != null)
                {
                    current = (cmbProvincie.SelectedItem as Provincie).Id;
                }
                bool? vlaandere = null;
                bool? wallonie = null;
                if (cbVlaanderen != null)
                {
                    vlaandere = cbVlaanderen.IsChecked;
                }
                if (cbWallonië != null)
                {
                    wallonie = cbWallonië.IsChecked;
                }
                var provinces = DatabaseOperations.GetProvincesByRegion(vlaandere, wallonie);
                cmbProvincie.ItemsSource = provinces;
                for (int i = 0; i < provinces.Count; i++)
                {
                    if (provinces[i].Id == current)
                    {
                        cmbProvincie.SelectedIndex = i;
                    }
                }
                FillClubs();
                //var currentInCmb = 
            }
        }
        //public List<Persoon> ZoekPersonen()
        //{
        //    var vlaandere = cbVlaanderen.IsChecked;
        //    var wallonie = cbWallonië.IsChecked;
        //    var zoekData = txtZoeken.Text;
        //    int? provincieId = null;
        //    if (cmbProvincie.SelectedItem != null)
        //    {
        //        provincieId = (cmbProvincie.SelectedItem as Provincie).Id;
        //    }
        //    var club = cmbClub.SelectedItem;

        //    //var Personen = AtletiekinfoContext.tblPersoon.Where(
        //    //    x => x.PersonenClubs.Where(y => y.begin.Year == DateTime.Now.Year && y.Club.Provincie.Id == provincieId).Any()
        //    //    );


        //    var Personen = AtletiekinfoContext.tblPersoon.AsQueryable();

        //    ////Add search data to query
        //    if (zoekData != null)
        //        //Personen = Personen.Where(p => );
        //        Personen = Personen.Where(p =>
        //        (p.voornaam + " " + p.naam).Contains(zoekData)
        //        || (p.naam + " " + p.voornaam).Contains(zoekData)
        //        || p.PersonenClubs.Where(y => y.begin.Year == DateTime.Now.Year && y.borstNummer.Contains(zoekData)).Any()
        //        );

        //    ////Add club to query

        //    return Personen.ToList();
        //}

        private void cmbProvincie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ZoekPersonen();
            //if (cmbProvincie.SelectedItem!=null)
            //{
                FillClubs();

            Search();
            //}
        }

        private void cmbClub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void cbVlaanderen_Change(object sender, RoutedEventArgs e)
        {
            if (cbVlaanderen.IsChecked == false && cbWallonië.IsChecked == false)
            {
                cbWallonië.IsChecked = true;
            }
            else
            {
                Search();
                FillProvince();
            }
        }

        private void cbWallonië_Change(object sender, RoutedEventArgs e)
        {
            if (cbWallonië.IsChecked == false && cbVlaanderen.IsChecked == false)
            {
                cbVlaanderen.IsChecked = true;
            }
            else
            {
                Search();
                FillProvince();
            }
        }

        private void dgPersonen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Hide();
            Profiel p = new Profiel((dgPersonen.SelectedItem as dto.Persoon).Id);
            p.Show();
            this.Close();
        }

        private void btnProfiel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Profiel p = new Profiel();
            p.Show();
            this.Close();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }
    }
}

