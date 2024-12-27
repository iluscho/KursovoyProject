using System;
using System.Linq;
using System.Windows;

namespace KursovoyProject
{
    public partial class AddVisitWindow : Window
    {
        private CarVisits _visit;
        private int _carID;

        public AddVisitWindow(CarVisits visit, int carID)
        {
            InitializeComponent();
            _visit = visit;
            _carID = carID;
            CarIDTextBox.Text = _carID.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var newCarVisit = new CarVisits
                    {
                        CarID = _carID,
                        Description = DescTextBox.Text,
                        Cost = 0,
                        Status = "In Progress",
                        VisitDate = DateTime.Now
                    };

                    if (int.TryParse(EmpIDTextBox.Text, out int empID))
                    {
                        newCarVisit.EmpID = empID;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректный номер сотрудника.");
                        return;
                    }

                    context.CarVisits.Add(newCarVisit);
                    context.SaveChanges();

                    MessageBox.Show("Запись успешно добавлена!");
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                string fullError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка добавления записи AddVisitWindow: {fullError}");
            }
        }
    }
}
