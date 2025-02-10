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
    }
}
