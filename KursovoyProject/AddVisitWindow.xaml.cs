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
    public partial class AddVisitWindow : Window
    {
        private CarVisits _visit;
        public AddVisitWindow(CarVisits visit)
        {
            InitializeComponent();
            _visit = visit;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var car = context.ClientCars.FirstOrDefault(c => c.CarID == _visit.CarID);

                    car.VIN = CarIDTextBox.Text;
                    //_visit.EmpID = EmpIDTextBox.Text;
                    //_visit.VisitDate = VisitDateTextBox.Text;
                    //_visit.Description = DescTextBox.Text;
                    //_visit.Cost = CostTextBox.Text;
                    _visit.Status = StatusTextBox.Text;
                    //_visit.RepairParts = RepairPartsListBox;

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
