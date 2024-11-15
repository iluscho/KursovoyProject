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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursovoyProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBox.ItemsSource = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" };
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var currentItems = listBox.ItemsSource as List<string>;
            currentItems.Add($"Item {currentItems.Count + 1}");
            listBox.ItemsSource = null;
            listBox.ItemsSource = currentItems;
        }


        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = listBox.SelectedItem as string;
            if (selectedItem != null)
            {
                var currentItems = listBox.ItemsSource as List<string>;
                currentItems.Remove(selectedItem);
                listBox.ItemsSource = null;
                listBox.ItemsSource = currentItems;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = new List<string>();
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = listBox.SelectedItem as string;
            if (selectedItem != null)
            {
                var detailWindow = new DetailWindow(selectedItem);
                detailWindow.ShowDialog();
            }
        }
    }
}
