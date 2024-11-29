using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для EditCarWindow.xaml
    /// </summary>
    public partial class EditCarWindow : Window
    {
        private ClientCars _car;

        public EditCarWindow(ClientCars car)
        {
            InitializeComponent();
            _car = car;

            LicensePlateTextBox.Text = _car.LicensePlate;
            BrandTextBox.Text = _car.Brand;
            ModelTextBox.Text = _car.Model;
            VINTextBox.Text = _car.VIN;
            YearTextBox.Text = Convert.ToString(_car.Year);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем объект новыми значениями
                _car.LicensePlate = LicensePlateTextBox.Text;
                _car.Brand = BrandTextBox.Text;
                _car.Model = ModelTextBox.Text;
                _car.VIN = VINTextBox.Text;
                if (Convert.ToInt32(YearTextBox.Text) > 1950 & Convert.ToInt32(YearTextBox.Text) < DateTime.Today.Year)
                {
                    _car.Year = Convert.ToInt32(YearTextBox.Text);
                    MessageBox.Show("Запись успешно обновлена.");
                }
                else
                {
                    MessageBox.Show("Введите корректный год автомобиля");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult = true;
            Close();
        }
    }

}
