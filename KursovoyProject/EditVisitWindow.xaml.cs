using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
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
    public partial class EditVisitWindow : Window
    {
        private CarVisits _visit;
        public EditVisitWindow(CarVisits visit)
        {
            InitializeComponent();
            var context = new IlyaServiceTemp1Entities();
            _visit = visit;

            var car = context.ClientCars.FirstOrDefault(c => c.CarID == _visit.CarID);

            CarIDTextBox.Text = car.VIN;
            EmpIDTextBox.Text = Convert.ToString(_visit.EmpID);
            VisitDateTextBox.Text = Convert.ToString(_visit.VisitDate);
            DescTextBox.Text = _visit.Description;
            //CostTextBox.Text = _visit.Cost;
            StatusTextBox.Text = _visit.Status;
            listBox = _visit.RepairParts;

            LoadVisitsFromDatabase(visit);

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
        private void LoadVisitsFromDatabase(Clients customer)
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

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCar = listBox.SelectedItem as string;
            if (selectedCar != null)
            {
                MessageBox.Show($"Вы выбрали: {selectedCar}");

                // Извлекаем подстроку, содержащую "ID:"
                int startIndex = selectedCar.IndexOf("ID:") + 3;
                if (startIndex >= 3) // Убедимся, что "ID:" найден
                {
                    // Находим конец числа
                    int endIndex = selectedCar.IndexOf(')', startIndex);
                    string id = selectedCar.Substring(startIndex, endIndex - startIndex).Trim();

                    MessageBox.Show($"ID: {id}");

                    // Преобразуем в число и открываем новое окно
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
