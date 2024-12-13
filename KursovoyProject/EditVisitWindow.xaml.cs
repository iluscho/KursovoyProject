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

            LoadVisitsFromDatabase(visit);

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //_visit.EmpID = EmpIDTextBox.Text;
                //_visit.VisitDate = VisitDateTextBox.Text;
                //_visit.Description = DescTextBox.Text;
                //_visit.Cost = CostTextBox.Text;
                _visit.Status = StatusTextBox.Text;
                //_visit.RepairParts = RepairPartsListBox;

                MessageBox.Show("Запись успешно обновлена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult = true; // Указываем, что изменения подтверждены
            Close();
        }
        private void LoadVisitsFromDatabase(CarVisits visit)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var visitsQuery = context.CarVisits
                        .Where(v => v.VisitID == visit.VisitID)
                        .ToList(); // Загружаем все записи в память

                    // Форматируем строку уже после того, как данные загружены
                    var formattedVisits = visitsQuery
                        .Select(v => $"{v.VisitID} - {v.VisitDate} {v.EmpID} ({v.CarID})")
                        .ToList();

                    listBox.ItemsSource = formattedVisits;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        //private void AddClientCarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (var context = new IlyaServiceTemp1Entities())
        //        {
        //            // Открываем окно добавления
        //            var newVisit = new CarVisits(); // Создаём новый объект
        //            var addWindow = new AddVisitWindow(newVisit, _visit); // Передаём его в окно

        //            if (addWindow.ShowDialog() == true) // Если пользователь подтвердил добавление
        //            {
        //                context.ClientCars.Add(newCar); // Добавляем новую запись в контекст
        //                context.SaveChanges(); // Сохраняем изменения в базе данных
        //                MessageBox.Show("Новая запись успешно добавлена.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
        //    }
        //}

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
