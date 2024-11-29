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
    }
}
