using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursovoyProject
{
    public partial class EditVisitWindow : Window
    {
        private CarVisits _visit;

        public EditVisitWindow(CarVisits visit)
        {
            InitializeComponent();
            _visit = visit;
            LoadVisitDetails();
        }

        private void LoadVisitDetails()
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    // Загрузка данных о визите
                    var car = context.ClientCars.FirstOrDefault(c => c.CarID == _visit.CarID);
                    var emp = context.Employees.FirstOrDefault(e => e.EmpID == _visit.EmpID);

                    if (car != null) CarIDTextBox.Text = car.VIN;
                    if (emp != null) EmpIDTextBox.Text = emp.FullName;

                    VisitDateTextBox.Text = _visit.VisitDate.ToString();
                    DescTextBox.Text = _visit.Description;
                    statusComboBox.Text = _visit.Status;

                    // Загрузка запчастей для визита
                    LoadPartsForVisit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных визита: {ex.Message}");
            }
        }

        private string GetPartNameById(int? partId)
        {
            using (var context = new IlyaServiceTemp1Entities())
            {
                var partName = context.AvailableParts
                                      .Where(p => p.PartID == partId)
                                      .Select(p => p.PartName)
                                      .FirstOrDefault();
                if (partName == null)
                {
                    MessageBox.Show($"Запчасть с ID {partId} не найдена.");
                }
                return partName ?? "Имя не найдено";
            }
        }

        private void LoadPartsForVisit()
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var partsUsed = context.RepairParts
                        .Where(rp => rp.VisitID == _visit.VisitID)
                        .ToList();

                    var formattedParts = partsUsed
                        .Select(p => $"{GetPartNameById(p.PartID)} {p.EstimatedCost} руб. - {p.Quantity} шт. (ID: {p.RepairPartID})")
                        .ToList();

                    listBox.ItemsSource = formattedParts;

                    var totalCost = partsUsed.Sum(p => p.EstimatedCost);
                    CostTextBox.Text = totalCost.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки запчастей: {ex.Message}");
            }
        }

        private void AddRepairPartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addWindow = new AddRepairPartWindow(_visit);
                if (addWindow.ShowDialog() == true)
                {
                    LoadPartsForVisit(); // Обновляем список запчастей
                    MessageBox.Show("Запчасть успешно добавлена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления запчасти: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var visitToUpdate = context.CarVisits.FirstOrDefault(v => v.VisitID == _visit.VisitID);
                    if (visitToUpdate != null)
                    {
                        visitToUpdate.Status = statusComboBox.Text;
                        context.SaveChanges();
                        MessageBox.Show("Визит успешно обновлен.");
                    }
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения визита: {ex.Message}");
            }
        }
    }
}
