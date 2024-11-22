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

        }

        private void AutoWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
