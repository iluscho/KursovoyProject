using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace KursovoyProject
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Server=192.168.147.54;Database=IlyaServiceTemp1;User Id=is;Password=1;";

        public MainWindow()
        {
            InitializeComponent();
            LoadCarsFromDatabase();
        }

        private void LoadCarsFromDatabase(string searchTerm = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT CONCAT(LicensePlate, ' - ', Brand, ' ', Model, ' (VIN: ', VIN, ')') AS CarDetails
                        FROM ClientCars
                        WHERE (@SearchTerm = '' OR LicensePlate LIKE '%' + @SearchTerm + '%' OR VIN LIKE '%' + @SearchTerm + '%')";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    SqlDataReader reader = command.ExecuteReader();
                    List<string> cars = new List<string>();

                    while (reader.Read())
                    {
                        cars.Add(reader.GetString(0));
                    }

                    listBox.ItemsSource = cars;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            LoadCarsFromDatabase(searchTerm);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавление новых записей в настоящее время не реализовано.");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                MessageBox.Show($"Удаление записи: {selectedCar}");
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Clear();
            LoadCarsFromDatabase();
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                MessageBox.Show($"Вы выбрали: {selectedCar}");
            }
        }
    }
}
