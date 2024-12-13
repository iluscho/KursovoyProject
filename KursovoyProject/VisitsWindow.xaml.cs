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
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class VisitsWindow : Window
    {
        private int _CarID = 0;
        public VisitsWindow(int CarID)
        {
            InitializeComponent();
            LoadVisitsFromDatabase(CarID);
            _CarID = CarID;
        }

        private void LoadVisitsFromDatabase(int CarID)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var visitsQuery = context.CarVisits
                        .Where(visit => visit.CarID == CarID)

                        .ToList(); // Загружаем все записи в память

                    // Форматируем строку уже после того, как данные загружены
                    var formattedVisits = visitsQuery
                        .Select(visit => $"{visit.CarID} {visit.VisitDate} {visit.Description} {visit.Cost} {visit.EmpID} (ID: {visit.VisitID})")
                        .ToList();

                    listBox.ItemsSource = formattedVisits;
                    var car = context.ClientCars.FirstOrDefault(c => c.CarID == CarID);
                    nameTextBox.Text = car.LicensePlate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AutoWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    // Открываем окно добавления
                    var newVisit = new CarVisits(); // Создаём новый объект
                    var addWindow = new AddVisitWindow(newVisit); // Передаём его в окно

                    if (addWindow.ShowDialog() == true) // Если пользователь подтвердил добавление
                    {
                        context.CarVisits.Add(newVisit); // Добавляем новую запись в контекст
                        context.SaveChanges(); // Сохраняем изменения в базе данных
                        MessageBox.Show("Новая запись успешно добавлена.");
                        
                        int CarID = _CarID;

                        LoadVisitsFromDatabase(CarID); // Обновляем список
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedVisit = listBox.SelectedItem as string;

            if (selectedVisit != null)
            {
                //var fullName = selectedVisit.Split(new[] { " - " }, StringSplitOptions.None)[0];

                try
                {
                    using (var context = new IlyaServiceTemp1Entities())
                    {
                        int startIndex = selectedVisit.IndexOf("ID:") + 3;
                        if (startIndex >= 3) // Убедимся, что "ID:" найден
                        {
                            // Находим конец числа
                            int endIndex = selectedVisit.IndexOf(')', startIndex);
                            string id = selectedVisit.Substring(startIndex, endIndex - startIndex).Trim();

                            MessageBox.Show($"ID: {id}");

                            var visit = context.CarVisits.FirstOrDefault(c => c.VisitID == (Convert.ToInt32(id)));

                            if (visit != null)
                            {
                                var editWindow = new EditVisitWindow(visit); // Передаём выбранный объект
                                if (editWindow.ShowDialog() == true) // Если изменения подтверждены
                                {
                                    context.SaveChanges(); // Сохраняем изменения в базе данных
                                    int VisitID = Convert.ToInt32(id);
                                    LoadVisitsFromDatabase(VisitID); // Обновляем список
                                }
                            }
                            else
                            {
                                MessageBox.Show("Запись не найдена.");
                            }
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
    }
}
