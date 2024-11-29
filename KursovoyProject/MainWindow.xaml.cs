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
                            // Предположим, у вас есть форма или диалоговое окно для редактирования данных
                            var editWindow = new EditCarWindow(car); // Передаём выбранный объект
                            if (editWindow.ShowDialog() == true) // Если изменения подтверждены
                            {
                                context.SaveChanges(); // Сохраняем изменения в базе данных
                                LoadCarsFromDatabase(); // Обновляем список
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
                MessageBox.Show($"Вы выбрали: {selectedCar}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    // Открываем окно добавления
                    var newCar = new ClientCars(); // Создаём новый объект
                    var addWindow = new AddCarWindow(newCar); // Передаём его в окно

                    if (addWindow.ShowDialog() == true) // Если пользователь подтвердил добавление
                    {
                        context.ClientCars.Add(newCar); // Добавляем новую запись в контекст
                        context.SaveChanges(); // Сохраняем изменения в базе данных
                        MessageBox.Show("Новая запись успешно добавлена.");
                        LoadCarsFromDatabase(); // Обновляем список
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
