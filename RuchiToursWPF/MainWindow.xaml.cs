
using MySql.Data.MySqlClient;

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

namespace RuchiToursWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private String username = "";
        private String password = "";

        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            SignUp SignupFm = new SignUp();
            SignupFm.Show();
            this.Hide();
        }


        private void TxtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            username = TxtUserName.Text;
        }

        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = TxtPassword.Text;

            if (username != "" && password != "")
            {
                BtnLogin.IsEnabled = true;
            }
            else
            {
                BtnLogin.IsEnabled = false;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            DBconnect db = new DBconnect();

            try
            {
                db.OpenConnection();
                using (MySqlConnection conn = db.GetConnection())
                {
                    string query = "SELECT COUNT(1) FROM user WHERE username=@username AND password=@password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 1)
                        {
                            this.Hide(); // Hide login form
                           // Session.Equals(username, password);
                            Home homePage = new Home();
                            homePage.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                db.CloseConnection();
            }

        }
    }
}
