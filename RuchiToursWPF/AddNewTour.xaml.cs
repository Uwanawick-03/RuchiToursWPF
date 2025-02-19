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
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace RuchiToursWPF
{
    /// <summary>
    /// Interaction logic for AddNewTour.xaml
    /// </summary>
    public partial class AddNewTour : UserControl
    {
        private int locationCount = 0;
        public AddNewTour()
        {
            InitializeComponent();

            LoadDataIntoComboBox();


            //this.ScrollViewer = true;
        }

        private void BtnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            AddNewLocationSection();
        }

        private void AddNewTour_Loaded(object sender, RoutedEventArgs e)
        {
            // Hide the initial controls
            ComBoxLocation.Visibility = Visibility.Collapsed;
            ComboxAccomo.Visibility = Visibility.Collapsed;
            DateTimePickerLocation.Visibility = Visibility.Collapsed;
            TxtCharge.Visibility = Visibility.Collapsed;
            accommodationCheck.Visibility = Visibility.Collapsed;
        }

        private void AddNewLocationSection()
        {
            // Increase locationCount to track the number of sections
            locationCount++;

            // Subsequent sections
            int sectionSpacing = 20;
            int sectionY = (int)(Canvas.GetTop(BtnAddLocation) + BtnAddLocation.Height + sectionSpacing + (locationCount - 1) * (120 + sectionSpacing)); // Adjust multiplier as needed for spacing


            // Create new controls for the section
            ComboBox newLocationBox = new ComboBox();
            // Set its position
            Canvas.SetLeft(newLocationBox, Canvas.GetLeft(ComBoxLocation)); // Same X position
            Canvas.SetTop(newLocationBox, sectionY); // Dynamic Y position
            newLocationBox.Width = ComBoxLocation.Width;
            newLocationBox.Text = "Location";
            newLocationBox.Visibility = Visibility.Visible;// Ensure visibility is set to true
            //newLocationBox.Name = "locationBox"+locationCount.ToString();
            AddNewTourGrid.Children.Add(newLocationBox);

            ComboBox newAccommodationBox = new ComboBox();
            Canvas.SetLeft(newAccommodationBox, Canvas.GetLeft(ComboxAccomo)); // Same X position
            Canvas.SetTop(newAccommodationBox, sectionY+30); // Dynamic Y position
            newAccommodationBox.Width = ComboxAccomo.Width;
            newAccommodationBox.Text = "Accommodation";
            newAccommodationBox.Visibility = Visibility.Visible;// Ensure visibility is set to true
            //newAccommodationBox.Name = "accommodationBox" + locationCount.ToString();
            AddNewTourGrid.Children.Add(newAccommodationBox);

            TextBox newTxtCharge = new TextBox();
            Canvas.SetLeft(newTxtCharge, Canvas.GetLeft(TxtCharge)); // Same X position
            Canvas.SetTop(newTxtCharge, sectionY + 60); // Dynamic Y position
            newTxtCharge.Width = TxtCharge.Width;
            newTxtCharge.Text = "Charge";
            //newTxtCharge.Name = "txtCharge" + locationCount.ToString();
            AddNewTourGrid.Children.Add(newTxtCharge);

            DatePicker newLocationDatePicker = new DatePicker();
            Canvas.SetLeft(newLocationDatePicker, Canvas.GetLeft(DateTimePickerLocation)); // Same X position
            Canvas.SetTop(newLocationDatePicker, sectionY + 90); // Dynamic Y position
            newLocationDatePicker.Width = DateTimePickerLocation.Width;
            // newLocationDatePicker.Name = "locationDatePicker" + locationCount.ToString();
            AddNewTourGrid.Children.Add(newLocationDatePicker);

            CheckBox newAccommodationCheck = new CheckBox();
            Canvas.SetLeft(newAccommodationCheck, Canvas.GetLeft(accommodationCheck)); // Same X position
            Canvas.SetTop(newAccommodationCheck, sectionY + 30);
            newAccommodationCheck.Content = "Already booked an Accommodation";
            newAccommodationCheck.Width = accommodationCheck.Width;
            //newAccommodationBox.Name = "accommodationCheck" + locationCount.ToString();
            newAccommodationCheck.Checked += (sender, e) =>
            {
                newAccommodationBox.IsEnabled = false; // Disable when checked
            };

            newAccommodationCheck.Unchecked += (sender, e) =>
            {
                newAccommodationBox.IsEnabled = true; // Enable when unchecked
            };
            newAccommodationCheck.Visibility = Visibility.Visible;// Ensure visibility is set to true
            AddNewTourGrid.Children.Add(newAccommodationCheck);

            // Optionally populate new comboboxes with data
            // LoadLocationData(newLocationBox);
            LoadLocationData(newLocationBox);
            LoadAccommodationData(newAccommodationBox);

            // Adjust Save button position after adding the new section
            AdjustSaveButtonPosition();

        }

        private void AdjustSaveButtonPosition()
        {
            // Calculate the Y position for the Save button
            int bottomMostSectionY = (int)Canvas.GetTop(BtnAddLocation) + (int)BtnAddLocation.Height + (locationCount) * (120 + 20);
            int saveButtonY = bottomMostSectionY + 50;

            // Create or move the Save button
            if (BtnSave == null)
            {
                BtnSave = new Button
                {
                    Content = "Save",
                    Width = 137, // Match width with the addLocationButton
                    Visibility = Visibility.Visible
                };

                //BtnSave.Click += SaveButton_Click; // Hook up save logic here
                AddNewTourGrid.Children.Add(BtnSave);
            }

            // Set position dynamically
            Canvas.SetLeft(BtnSave, 740);
            Canvas.SetTop(BtnSave, saveButtonY);
        }

        private void LoadDataIntoComboBox()
        {
            LoadGuestData();
            LoadDriverData();
            LoadGuideData();
            LoadVehicleData();
            LoadCurrencyData();
            LoadLocationData(ComBoxLocation);
            LoadAccommodationData(ComboxAccomo);
        }

        private void LoadGuestData()
        {
            DBconnect db = new DBconnect();
            string query = "SELECT guestId, fullName FROM guest";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["fullName"].ToString(),
                                    Tag = reader["guestId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxGuest.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading guest data: " + ex.Message);
                }
            }
        }

        private void LoadDriverData()
        {
            DBconnect db = new DBconnect();
            string query = "SELECT driverId, fullName FROM driver";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["fullName"].ToString(),
                                    Tag = reader["driverId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxDriver.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading driver data: " + ex.Message);
                }
            }
        }

        private void LoadGuideData()
        {
            DBconnect db = new DBconnect();
            string query = "SELECT guideId, fullName FROM guide";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["fullName"].ToString(),
                                    Tag = reader["guideId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxGuide.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading guide data: " + ex.Message);
                }
            }
        }

        private void LoadVehicleData()
        {
            DBconnect db = new DBconnect();
            string query = "SELECT vehicleId, vehicleType FROM vehicle";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["vehicleType"].ToString(),
                                    Tag = reader["vehicleId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxVehicle.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicle data: " + ex.Message);
                }
            }
        }

        private void LoadCurrencyData()
        {
            DBconnect db = new DBconnect();
            string query = "SELECT currencyId, currency FROM currency";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["currency"].ToString(),
                                    Tag = reader["currencyId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxCurrency.Items.Add(item);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading currency data: " + ex.Message);
                }
            }
        }

        private void LoadLocationData(ComboBox locationComboBox)
        {
            DBconnect db = new DBconnect();
            string query = "SELECT locationId, locationName FROM location";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["locationName"].ToString(),
                                    Tag = reader["locationId"].ToString()  // Store guestId in the Tag property
                                };
                                ComBoxLocation.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading location data: " + ex.Message);
                }
            }
        }

        private void LoadAccommodationData(ComboBox accommodationComboBox)
        {
            DBconnect db = new DBconnect();
            string query = "SELECT accomadationId, hotelName FROM accommodation";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a ComboBoxItem
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["hotelName"].ToString(),
                                    Tag = reader["accomadationId"].ToString()  // Store guestId in the Tag property
                                };
                                ComboxAccomo.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading accommodation data: " + ex.Message);
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DBconnect db = new DBconnect();

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    db.OpenConnection();

                    string driverId = (ComBoxDriver.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                    string guideId = (ComBoxGuide.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                    string guestId = (ComBoxGuest.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                    string currencyId = (ComBoxCurrency.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                    string noOfMembers = TxtNumberOfTM.Text;
                    string vehicleId = (ComBoxVehicle.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                    DateTime startDate = DateTimePickerStartDate.SelectedDate ?? DateTime.MinValue;
                    DateTime endDate = DateTimePickerEndingDate.SelectedDate ?? DateTime.MinValue;

                    string status = DetermineTourStatus(startDate, endDate);

                    int createdUserId = SessionData.UserID;

                    // Calculate the total charge
                    decimal totalCharge = 0;
                    for (int i = 0; i < locationCount; i++)
                    {
                        TextBox chargeBox = this.AddNewTourGrid.Children.OfType<TextBox>().FirstOrDefault(c => Canvas.GetTop(c) == (Canvas.GetTop(BtnAddLocation) + BtnAddLocation.Height + 20 + i * (120 + 20) + 60));
                        if (chargeBox != null)
                        {
                            if (decimal.TryParse(chargeBox.Text, out decimal charge))
                            {
                                totalCharge += charge;
                            }
                        }
                    }

                    string mainTableQuery = @"INSERT INTO tour (numberOfTourMembers, startingDate, endingDate, tourStatus, totalTourCharge, currencyId, driverId, guestId, guideId, vehicleId, createdUserId) 
                                      VALUES (@num, @startingD, @endingD, @status, @totalCharge, @currencyId, @driverId, @guestId, @guideId, @vehicleId, @createdUserId)";

                    using (MySqlCommand mainTableCmd = new MySqlCommand(mainTableQuery, conn))
                    {
                        mainTableCmd.Parameters.AddWithValue("@driverId", driverId);
                        mainTableCmd.Parameters.AddWithValue("@guideId", guideId);
                        mainTableCmd.Parameters.AddWithValue("@guestId", guestId);
                        mainTableCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                        mainTableCmd.Parameters.AddWithValue("@currencyId", currencyId);
                        mainTableCmd.Parameters.AddWithValue("@num", noOfMembers);
                        mainTableCmd.Parameters.AddWithValue("@startingD", startDate);
                        mainTableCmd.Parameters.AddWithValue("@endingD", endDate);
                        mainTableCmd.Parameters.AddWithValue("@status", status); // Add status parameter
                        mainTableCmd.Parameters.AddWithValue("@totalCharge", totalCharge); // Add total charge parameter
                        mainTableCmd.Parameters.AddWithValue("@createdUserId", createdUserId);
                        mainTableCmd.ExecuteNonQuery();
                    }

                    // Get the ID of the newly inserted row in the main table
                    // After inserting into the main 'tour' table
                    long mainTableId = 0;
                    string getLastInsertedIdQuery = "SELECT LAST_INSERT_ID()";
                    using (MySqlCommand getIdCmd = new MySqlCommand(getLastInsertedIdQuery, conn))
                    {
                        var result = getIdCmd.ExecuteScalar();
                        if (result is ulong)
                        {
                            mainTableId = (long)(ulong)result; // Safe conversion with check
                        }
                        else if (result is long)
                        {
                            mainTableId = (long)result;
                        }
                        else
                        {
                            throw new InvalidCastException("Unexpected type returned for LAST_INSERT_ID()");
                        }
                    }

                    // Insert into 'tour_has_location' table
                    for (int i = 0; i < locationCount; i++)
                    {
                        int sectionSpacing = 20;
                        int sectionY = (int)Canvas.GetTop(BtnAddLocation) + (int)BtnAddLocation.Height + sectionSpacing + (i) * (120 + sectionSpacing);

                        // Ensure controls are correctly fetched from AddNewTourGrid
                        ComboBox locationBox = AddNewTourGrid.Children.OfType<ComboBox>()
                            .FirstOrDefault(c => Canvas.GetTop(c) == sectionY);

                        ComboBox accommodationBox = AddNewTourGrid.Children.OfType<ComboBox>()
                            .FirstOrDefault(c => Canvas.GetTop(c) == sectionY + 30);

                        TextBox chargeBox = AddNewTourGrid.Children.OfType<TextBox>()
                            .FirstOrDefault(c => Canvas.GetTop(c) == sectionY + 60);

                        DatePicker datePicker = AddNewTourGrid.Children.OfType<DatePicker>()
                            .FirstOrDefault(c => Canvas.GetTop(c) == sectionY + 90);

                        CheckBox accommodationCheck = AddNewTourGrid.Children.OfType<CheckBox>()
                            .FirstOrDefault(c => Canvas.GetTop(c) == sectionY + 30);

                        if (locationBox == null || accommodationBox == null || chargeBox == null || datePicker == null || accommodationCheck == null)
                            continue;

                        // Fetch values
                        string locationId = (locationBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                        string accommodationId = accommodationCheck.IsChecked == true ? null : accommodationBox.SelectedValue?.ToString();
                        string charge = chargeBox.Text;
                        DateTime? selectedDate = datePicker.SelectedDate;

                        // Insert into 'tour_has_location'
                        string insertLocationQuery = @"INSERT INTO tour_has_location 
                                   (tourId, locationId, accommodationId, charge, date,createdUserId) 
                                   VALUES 
                                   (@tourId, @locationId, @accommodationId, @charge, @date, @createdUserId)";

                        using (MySqlCommand insertLocationCmd = new MySqlCommand(insertLocationQuery, conn))
                        {
                            insertLocationCmd.Parameters.AddWithValue("@tourId", mainTableId);
                            insertLocationCmd.Parameters.AddWithValue("@locationId", locationId);
                            insertLocationCmd.Parameters.AddWithValue("@accommodationId", (object)accommodationId ?? DBNull.Value);
                            insertLocationCmd.Parameters.AddWithValue("@charge", charge);
                            insertLocationCmd.Parameters.AddWithValue("@date", selectedDate.Value);
                            insertLocationCmd.Parameters.AddWithValue("@createdUserId", createdUserId);

                            insertLocationCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private string DetermineTourStatus(DateTime startDate, DateTime endDate)
        {
            // Determine current date
            DateTime currentDate = DateTime.Now;

            // Determine tour status based on dates
            if (currentDate < startDate)
            {
                return "upcoming";
            }
            else if (currentDate >= startDate && currentDate <= endDate)
            {
                return "ongoing";
            }
            else
            {
                return "ended";
            }
        }

        /*private void ComBoxVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }*/
    }
}