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
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace RuchiToursWPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult check = MessageBox.Show("Are You sure you want to logout?"
                ,"Confirmation Message",MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (check == MessageBoxResult.Yes)
            {
                MainWindow loginForm = new MainWindow();
                loginForm.Show();
                this.Hide();
            }
        }

        private void BtnAddNewTour_Click(object sender, RoutedEventArgs e)
        {
           
            viewTours.IsVisible = false;
            guestDetails1.Visible = false;
            drivers1.Visible = false;
            guides1.Visible = false;
            vehicle1.Visible = false;
            currency1.Visible = false;
            locations1.Visible = false;
            accommodations1.Visible = false;
            addAccommodation1.Visible = false;
            addLocations1.Visible = false;
            addCurrency1.Visible = false;
            addVehicles1.Visible = false;
            addGuides1.Visible = false;
            addDrivers1.Visible = false;
            addGuestDetails1.Visible = false;
        }
    }
}
