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
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            LoadCustomersFromDatabase();
        }

        private void LoadCustomersFromDatabase(string searchTerm = "")
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var customersQuery = context.Clients
                        .Where(customer => string.IsNullOrEmpty(searchTerm) ||
                                      customer.FullName.Contains(searchTerm) ||
                                      customer.Phone.Contains(searchTerm) ||
                                      customer.Address.Contains(searchTerm) ||
                                      customer.Email.Contains(searchTerm))

                        .ToList(); // Загружаем все записи в память

                    // Форматируем строку уже после того, как данные загружены
                    var formattedCustomers = customersQuery
                        .Select(customer => $"{customer.FullName} - {customer.Phone} {customer.Address} (ID: {customer.ClientID})")
                        .ToList();

                    listBox.ItemsSource = formattedCustomers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            LoadCustomersFromDatabase(searchTerm);
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditButton_Click(sender, e);
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
                        LoadCustomersFromDatabase(); // Обновляем список
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
                                LoadCustomersFromDatabase(); // Обновляем список
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
