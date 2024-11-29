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
    public partial class AddCustomerWindow : Window
    {
        private Clients _customer;
        public AddCustomerWindow(Clients client)
        {
            InitializeComponent();
            _customer = client;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    //_customer.ClientID = client.ClientID;
                    
                    _customer.FullName = FullNameTextBox.Text;
                    _customer.Phone = PhoneTextBox.Text;
                    _customer.Email = EmailTextBox.Text;
                    _customer.Address = AdressTextBox.Text;

                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления записи: {ex.Message}");
            }
        }
    }
}
