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
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class VisitsWindow : Window
    {
        public VisitsWindow(int CarID)
        {
            InitializeComponent();
            LoadVisitsFromDatabase(CarID);
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
                        .Select(visit => $"{visit.CarID} {visit.VisitID} {visit.VisitDate} {visit.Description} {visit.Cost} {visit.EmpID}")
                        .ToList();

                    listBox.ItemsSource = formattedVisits;
                    searchTextBox.Text = "ТУТ НАЗВАНИЕ МАШИНЫ ВСТАВИТЬ";
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
                    var newCustomer = new Clients(); // Создаём новый объект
                    var addWindow = new AddCustomerWindow(newCustomer); // Передаём его в окно

                    if (addWindow.ShowDialog() == true) // Если пользователь подтвердил добавление
                    {
                        context.Clients.Add(newCustomer); // Добавляем новую запись в контекст
                        context.SaveChanges(); // Сохраняем изменения в базе данных
                        MessageBox.Show("Новая запись успешно добавлена.");

                        int CarID = 0; //ИЗМЕНИТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
            var selectedCustomer = listBox.SelectedItem as string;

            if (selectedCustomer != null)
            {
                var fullName = selectedCustomer.Split(new[] { " - " }, StringSplitOptions.None)[0];

                try
                {
                    using (var context = new IlyaServiceTemp1Entities())
                    {
                        var customer = context.Clients.FirstOrDefault(c => c.FullName == fullName);

                        if (customer != null)
                        {
                            var editWindow = new EditCustomerWindow(customer); // Передаём выбранный объект
                            if (editWindow.ShowDialog() == true) // Если изменения подтверждены
                            {
                                context.SaveChanges(); // Сохраняем изменения в базе данных
                                int CarID = 0; //ИЗМЕНИТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                LoadVisitsFromDatabase(CarID); // Обновляем список
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
    }
}
