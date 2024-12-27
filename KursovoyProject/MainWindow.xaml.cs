using System;
using System.Linq;
using System.Windows;

namespace KursovoyProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCarsFromDatabase();
        }

        private void LoadCarsFromDatabase(string searchTerm = "")
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var carsQuery = context.ClientCars
                        .Where(car => string.IsNullOrEmpty(searchTerm) ||
                                      car.LicensePlate.Contains(searchTerm) ||
                                      car.VIN.Contains(searchTerm))
                        .ToList();

                    var formattedCars = carsQuery
                        .Select(car => $"{car.LicensePlate} - {car.Brand} {car.Model} (VIN: {car.VIN})  (ID: {car.CarID}) ")
                        .ToList();

                    listBox.ItemsSource = formattedCars;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void CustomersWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CustomersWindow window = new CustomersWindow();
            window.Show();
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;

            if (selectedCar != null)
            {
                var licensePlate = selectedCar.Split(new[] { " - " }, StringSplitOptions.None)[0];

                try
                {
                    using (var context = new IlyaServiceTemp1Entities())
                    {
                        var car = context.ClientCars.FirstOrDefault(c => c.LicensePlate == licensePlate);

                        if (car != null)
                        {
                            var editWindow = new EditCarWindow(car);
                            if (editWindow.ShowDialog() == true) 
                            {
                                context.SaveChanges(); 
                                LoadCarsFromDatabase();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка редактирования записи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                int startIndex = selectedCar.IndexOf("ID:") + 3;
                if (startIndex >= 3)
                {
                    int endIndex = selectedCar.IndexOf(')', startIndex);
                    string id = selectedCar.Substring(startIndex, endIndex - startIndex).Trim();

                    if (int.TryParse(id, out int idint))
                    {
                        Window VisitsWindow = new VisitsWindow(idint);
                        VisitsWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: ID не является числом.");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка: ID не найден.");
                }


            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var newCar = new ClientCars();
                    Clients client = new Clients();
                    var addWindow = new AddCarWindow(newCar, client);

                    if (addWindow.ShowDialog() == true)
                    {
                        context.ClientCars.Add(newCar);
                        context.SaveChanges();
                        MessageBox.Show("Новая запись успешно добавлена.");
                        LoadCarsFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
            }
        }


        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            LoadCarsFromDatabase(searchTerm);
        }

        private void listBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
