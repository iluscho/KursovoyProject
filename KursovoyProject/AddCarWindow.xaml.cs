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

namespace KursovoyProject
{
    /// <summary>
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        private ClientCars _car;
        public AddCarWindow(ClientCars car)
        {
            InitializeComponent();
            _car = car;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    // Ищем клиента по имени
                    var client = context.Clients.FirstOrDefault(c => c.FullName == FullNameTextBox.Text);

                    if (client != null)
                    {
                        _car.ClientID = client.ClientID; // Присваиваем найденный ID
                    }
                    else
                    {
                        MessageBox.Show("Клиент с таким именем не найден. Проверьте правильность ввода.");
                        return; // Останавливаем сохранение, если клиент не найден
                    }

                    // Заполняем остальные поля
                    _car.LicensePlate = LicensePlateTextBox.Text;
                    _car.Brand = BrandTextBox.Text;
                    _car.Model = ModelTextBox.Text;

                    //_car.VIN = VINTextBox.Text;

                    if (VINTextBox.Text.Length == 17)
                    {
                        _car.VIN = VINTextBox.Text;
                        MessageBox.Show("Запись успешно подготовлена для добавления.");
                        DialogResult = true; // Закрываем окно с подтверждением
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный VIN автомобиля.");
                    }

                    if (int.TryParse(YearTextBox.Text, out int year) && year > 1950 && year <= DateTime.Now.Year)
                    {
                        _car.Year = year;
                        MessageBox.Show("Запись успешно подготовлена для добавления.");
                        DialogResult = true; // Закрываем окно с подтверждением
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный год выпуска автомобиля.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
            }
        }

    }
}
