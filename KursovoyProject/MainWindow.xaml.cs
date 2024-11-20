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
                        .ToList(); // Загружаем все записи в память

                    // Форматируем строку уже после того, как данные загружены
                    var formattedCars = carsQuery
                        .Select(car => $"{car.LicensePlate} - {car.Brand} {car.Model} (VIN: {car.VIN})")
                        .ToList();

                    listBox.ItemsSource = formattedCars;
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

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Clear();
            LoadCarsFromDatabase();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
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
                            context.ClientCars.Remove(car);
                            context.SaveChanges();
                            MessageBox.Show("Запись успешно удалена.");
                            LoadCarsFromDatabase(); // Обновляем список
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления записи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                MessageBox.Show($"Вы выбрали: {selectedCar}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
