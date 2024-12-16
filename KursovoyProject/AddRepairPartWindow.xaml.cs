using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KursovoyProject
{
    public partial class AddRepairPartWindow : Window
    {
        private CarVisits _visit;

        public int SelectedPartID { get; private set; }
        public int SelectedQuantity { get; private set; }
        public decimal SelectedPartPrice { get; private set; }

        public AddRepairPartWindow(CarVisits visit)
        {
            InitializeComponent();
            _visit = visit;
            VisitIDTextBox.Text = _visit.VisitID.ToString();
            LoadPartsFromDatabase();
        }

        private void LoadPartsFromDatabase(string searchTerm = "")
        {
            try
            {
                using (var context = new IlyaServiceTemp1Entities())
                {
                    var partsQuery = context.AvailableParts
                        .Where(part => string.IsNullOrEmpty(searchTerm) || part.PartName.Contains(searchTerm))
                        .ToList();

                    var formattedParts = partsQuery
                        .Select(p => $"{p.PartName} - {p.Price:F2} руб. (ID: {p.PartID})")
                        .ToList();

                    listBox.ItemsSource = formattedParts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки доступных запчастей: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPart = listBox.SelectedItem as string;

            if (selectedPart != null)
            {
                try
                {
                    int startIndex = selectedPart.IndexOf("ID:") + 3;
                    int endIndex = selectedPart.IndexOf(')', startIndex);

                    if (int.TryParse(selectedPart.Substring(startIndex, endIndex - startIndex).Trim(), out int partID) &&
                        int.TryParse(PartQuant.Text, out int quantity))
                    {
                        using (var context = new IlyaServiceTemp1Entities())
                        {
                            var selectedPartEntity = context.AvailableParts.FirstOrDefault(p => p.PartID == partID);

                            if (selectedPartEntity != null)
                            {
                                SelectedPartID = partID;
                                SelectedQuantity = quantity;
                                SelectedPartPrice = selectedPartEntity.Price;

                                // Добавление новой запчасти
                                var repairPart = new RepairParts
                                {
                                    VisitID = _visit.VisitID,
                                    PartID = partID,
                                    Quantity = quantity,
                                    EstimatedCost = quantity * selectedPartEntity.Price
                                };

                                context.RepairParts.Add(repairPart);
                                context.SaveChanges();
                                MessageBox.Show("Запчасть успешно добавлена.");
                                DialogResult = true;
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Выбранная запчасть не найдена.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные данные.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления запчасти: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите запчасть из списка.");
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadPartsFromDatabase(searchTextBox.Text.Trim());
        }
    }
}
