using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using MySql.Data.MySqlClient;

namespace RuchiToursWPF
{
    /// <summary>
    /// Interaction logic for GuestDetails.xaml
    /// </summary>
    public partial class GuestDetails : UserControl
    {
        public ObservableCollection<Guest> Guests { get; set; }

        public GuestDetails()
        {
            InitializeComponent();
            Guests = new ObservableCollection<Guest>();
            LoadData();
            guestDataGridView.ItemsSource = Guests;
        }

        public void LoadData()
        {
            string connectionString = "Server=localhost;Database=rt_app;User Id=root;Password=Mang#301;";
            string query = "SELECT guestId, fullName, contactNO, email, country, passportNO FROM guest";

            Guests.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guests.Add(new Guest
                            {
                                GuestId = reader.GetInt32("guestId"),
                                FullName = reader.GetString("fullName"),
                                ContactNo = reader.GetString("contactNO"),
                                Email = reader.GetString("email"),
                                Country = reader.GetString("country"),
                                PassportNo = reader.GetString("passportNO")
                            });
                        }
                    }
                }
            }
        }



        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the data context (the guest object associated with the row)
            var button = sender as Button;
            var guest = button?.DataContext as Guest;

            if (guest != null)
            {
                // Show the edit dialog (or edit form, depending on your application)
                var editForm = new EditGuestDetails(guest.GuestId, guest.FullName, guest.ContactNo, guest.Email, guest.Country, guest.PassportNo);
                if (editForm.ShowDialog() == true)
                {
                    // Update the guest's data in the ObservableCollection (automatically updates the DataGrid)
                    guest.FullName = editForm.FullName;
                    guest.ContactNo = editForm.ContactNo;
                    guest.Email = editForm.Email;
                    guest.Country = editForm.Country;
                    guest.PassportNo = editForm.PassportNo;

                    // Update the database if needed
                    UpdateDatabase(guest.GuestId, guest.FullName, guest.ContactNo, guest.Email, guest.Country, guest.PassportNo);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the data context (the guest object associated with the row)
            var button = sender as Button;
            var guest = button?.DataContext as Guest;

            if (guest != null)
            {
                // Confirm the deletion
                var result = MessageBox.Show($"Are you sure you want to delete guest: {guest.FullName}?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove the guest from the ObservableCollection (automatically updates the DataGrid)
                    Guests.Remove(guest);

                    // Delete from the database
                    DeleteFromDatabase(guest.GuestId);
                }
            }
        }

        private void UpdateDatabase(int id, string fullName, string contactNo, string email, string country, string passportNo)
        {
            string connectionString = "Server=localhost;Database=rt_app;User Id=root;Password=Mang#301;";
            string query = "UPDATE guest SET fullName = @fullName, contactNO = @contactNo, email = @Email, country = @Country, passportNO = @PassportNo WHERE guestId = @Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fullName", fullName);
                    cmd.Parameters.AddWithValue("@contactNo", contactNo);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Parameters.AddWithValue("@PassportNo", passportNo);
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteFromDatabase(int id)
        {
            string connectionString = "Server=localhost;Database=rt_app;User Id=root;Password=Mang#301;";
            string query = "DELETE FROM guest WHERE guestId = @Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        private void TxtSearchBoxGD_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TxtSearchBoxGD.Text.ToLower();
            guestDataGridView.ItemsSource = Guests.Where(g =>
                g.FullName.ToLower().Contains(searchText) ||
                g.ContactNo.ToLower().Contains(searchText) ||
                g.Email.ToLower().Contains(searchText) ||
                g.Country.ToLower().Contains(searchText) ||
                g.PassportNo.ToLower().Contains(searchText)).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddGuestDetails guestDetails = new AddGuestDetails();
            this.Visibility = Visibility.Collapsed;
            guestDetails.Visibility = Visibility.Visible;
        }

        public class Guest
        {
            public int GuestId { get; set; }
            public string FullName { get; set; }
            public string ContactNo { get; set; }
            public string Email { get; set; }
            public string Country { get; set; }
            public string PassportNo { get; set; }
        }
    }
}
