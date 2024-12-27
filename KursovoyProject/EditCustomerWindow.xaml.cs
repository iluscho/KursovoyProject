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

            DialogResult = true;
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

        private void AddClientCarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var newCar = new ClientCars();
                    var addWindow = new AddCarWindow(newCar, _customer);

                    if (addWindow.ShowDialog() == true)
                    {
                        context.ClientCars.Add(newCar);
                        context.SaveChanges();
                        MessageBox.Show("Новая запись успешно добавлена.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                MessageBox.Show($"Вы выбрали: {selectedCar}");

                int startIndex = selectedCar.IndexOf("ID:") + 3;
                if (startIndex >= 3)
                {
                    int endIndex = selectedCar.IndexOf(')', startIndex);
                    string id = selectedCar.Substring(startIndex, endIndex - startIndex).Trim();

                    MessageBox.Show($"ID: {id}");

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
    }
}
