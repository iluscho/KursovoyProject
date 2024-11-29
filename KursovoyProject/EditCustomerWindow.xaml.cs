using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// Логика взаимодействия для EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        private Clients _customer;
        public EditCustomerWindow(Clients customer)
        {
            InitializeComponent();
            _customer = customer;

            FullNameTextBox.Text = _customer.FullName;
            PhoneTextBox.Text = _customer.Phone;
            EmailTextBox.Text = _customer.Email;
            AdressTextBox.Text = _customer.Address;
            LoadCarsFromDatabase(customer);

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _customer.FullName = FullNameTextBox.Text;
                _customer.Phone = PhoneTextBox.Text;
                _customer.Email = EmailTextBox.Text;
                _customer.Address = AdressTextBox.Text;

                MessageBox.Show("Запись успешно обновлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult = true; // Указываем, что изменения подтверждены
            Close();
        }
        private void LoadCarsFromDatabase(Clients customer)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var carsQuery = context.ClientCars
                        .Where(car => car.ClientID == customer.ClientID)
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

        private void AddClientCarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    // Открываем окно добавления
                    var newCar = new ClientCars(); // Создаём новый объект
                    var addWindow = new AddCarWindow(newCar, _customer); // Передаём его в окно

                    if (addWindow.ShowDialog() == true) // Если пользователь подтвердил добавление
                    {
                        context.ClientCars.Add(newCar); // Добавляем новую запись в контекст
                        context.SaveChanges(); // Сохраняем изменения в базе данных
                        MessageBox.Show("Новая запись успешно добавлена.");
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
