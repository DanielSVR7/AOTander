using AOTander.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace AOTander.Views
{
    public partial class MainWindow : Window
    {
        DateTime WorkTimer;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            WorkTimer = WorkTimer.AddSeconds(1);
            timerTB.Text = WorkTimer.ToString("HH:mm:ss");
        }

        private void EmployeesFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text_box = (TextBox)sender;
            var collection = (CollectionViewSource)text_box.FindResource("EmployeesCollection");
            if (collection.Source != null)
            {
                collection.View.Refresh();
                CountTextBox.Text = (collection.View.Cast<object>().Count() - 1).ToString();
            }
        }

        private void PosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo_box = (ComboBox)sender;
            var collection = (CollectionViewSource)combo_box.FindResource("EmployeesCollection");
            if (collection.Source != null)
            {
                collection.View.Refresh();
                CountTextBox.Text = (collection.View.Cast<object>().Count() - 1).ToString();
            }
        }

        private void ResetEmployeesFilterButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesFilterTextBox.Text = string.Empty;
            PosComboBox.SelectedIndex = -1;
        }

        private void EmployeesCollection_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Employees employee)) return;
            var filter_text = EmployeesFilterTextBox.Text.ToLower().Trim();
            if (filter_text.Length == 0 && PosComboBox.SelectedIndex == -1) 
                return;
            if (PosComboBox.SelectedIndex != -1)
            {
                if (employee.Positions == PosComboBox.SelectedItem) 
                    e.Accepted = true;
                else 
                    e.Accepted = false;
            }
            if (e.Accepted == true)
            {
                if (filter_text.Length > 0)
                {
                    if (employee.Surname.ToLower().StartsWith(filter_text)) return;
                    if (employee.Name.ToLower().StartsWith(filter_text)) return;
                    if (employee.Patronymic.ToLower().StartsWith(filter_text)) return;
                    if (employee.Birthday.ToString().Contains(filter_text)) return;
                    if (employee.Phone.Contains(filter_text)) return;
                }
                else 
                    return;
            }
            e.Accepted = false;
        }

        private void ShopsCollection_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Shops shop)) return;
            var filter_text = ShopsFilterTextBox.Text.ToLower().Trim();
            if (filter_text.Length == 0) return;
            if (shop.Address.ToLower().Contains(filter_text)) return;
            e.Accepted = false;
        }

        private void ShopsFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text_box = (TextBox)sender;
            var collection = (CollectionViewSource)text_box.FindResource("ShopsCollection");
            collection.View.Refresh();
        }

        private void ResetShopsFilterutton_Click(object sender, RoutedEventArgs e)
        {
            ShopsFilterTextBox.Text = string.Empty;
        }
    }
}
