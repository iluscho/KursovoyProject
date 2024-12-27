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
        private Clients _customer;
        public AddCarWindow(ClientCars car, Clients customer)
        {
            InitializeComponent();
            _car = car;
            _customer = customer; 
            FullNameTextBox.Text = _customer.FullName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var client = context.Clients.FirstOrDefault(c => c.FullName == FullNameTextBox.Text);

                    if (client != null)
                    {
                        _car.ClientID = client.ClientID; 
                    }
                    else
                    {
                        MessageBox.Show("Клиент с таким именем не найден. Проверьте правильность ввода.");
                        return; 
                    }

                    _car.LicensePlate = LicensePlateTextBox.Text;
                    _car.Brand = BrandTextBox.Text;
                    _car.Model = ModelTextBox.Text;


                    if (VINTextBox.Text.Length == 17)
                    {
                        _car.VIN = VINTextBox.Text;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный VIN автомобиля.");
                    }

                    if (int.TryParse(YearTextBox.Text, out int year) && year > 1950 && year <= DateTime.Now.Year)
                    {
                        _car.Year = year;
                        DialogResult = true;
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

        private void VINTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
