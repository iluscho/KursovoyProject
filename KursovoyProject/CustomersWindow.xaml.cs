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

                        .ToList();

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
                    var newCustomer = new Clients();
                    var addWindow = new AddCustomerWindow(newCustomer);

                    if (addWindow.ShowDialog() == true)
                    {
                        context.Clients.Add(newCustomer);
                        context.SaveChanges();
                        MessageBox.Show("Новая запись успешно добавлена.");
                        LoadCustomersFromDatabase();
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
                            var editWindow = new EditCustomerWindow(customer);
                            if (editWindow.ShowDialog() == true)
                            {
                                context.SaveChanges();
                                LoadCustomersFromDatabase();
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
