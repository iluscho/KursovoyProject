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
    /// Логика взаимодействия для AddRepairPartWindow.xaml
    /// </summary>
    public partial class AddRepairPartWindow : Window
    {
        private CarVisits _visit;
        public AddRepairPartWindow(CarVisits visit)
        {
            InitializeComponent();
            _visit = visit;
            VisitIDTextBox.Text = Convert.ToString(visit.VisitID);
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
                        .ToList(); // Загружаем все записи в память

                    // Форматируем строку уже после того, как данные загружены
                    var formattedParts = partsQuery
                        .Select(p => $"{p.PartName} - {p.Price} (ID: {p.PartID}")
                        .ToList();

                    listBox.ItemsSource = formattedParts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            var selectedPart = listBox.SelectedItem as string;

            if (selectedPart != null)
            {

                try
                {
                    using (var context = new IlyaServiceTemp1Entities())
                    {
                        int startIndex = selectedPart.IndexOf("ID:") + 3;
                        if (startIndex >= 3) // Убедимся, что "ID:" найден
                        {
                            // Находим конец числа
                            int endIndex = selectedPart.IndexOf(')', startIndex);
                            string id = selectedPart.Substring(startIndex, endIndex - startIndex).Trim();

                            if (int.TryParse(id, out int PartID))
                            {
                                // Используем VisitID, который уже преобразован в int
                                var part = context.RepairParts.FirstOrDefault(c => c.RepairPartID == PartID);

                                if (part != null)
                                {
                                    var editWindow = new EditVisitWindow(part); // Передаём выбранный объект /// ТУТ НА БУМ СДЕЛАЛ, починить!!!!!!!!!!!!!!!!!!!!!!!
                                    if (editWindow.ShowDialog() == true) // Если изменения подтверждены
                                    {
                                        context.SaveChanges(); // Сохраняем изменения в базе данных
                                        LoadPartsFromDatabase(); // Обновляем список
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Запись не найдена.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Некорректный формат ID.");
                            }

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


        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            LoadPartsFromDatabase(searchTerm);
        }
    }
}
