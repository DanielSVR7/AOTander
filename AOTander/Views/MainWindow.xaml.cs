using AOTander.Models;
using AOTander.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace AOTander.Views
{
    public partial class MainWindow : Window
    {
        DateTime WorkTimer;
        readonly DispatcherTimer timer;
        readonly MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = new MainWindowViewModel();
            this.DataContext = vm;
            if (vm.ExitAccountAction == null)
                vm.ExitAccountAction = new Action(() => ExitAccount());
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void ExitAccount()
        {
            AuthorizationWindow w = new AuthorizationWindow();
            w.Show();
            this.Close();
        }
        private void Timer_Tick(object sender, EventArgs e)
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

        private void LoginsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginsWindow w = new LoginsWindow() { Owner = this };
            w.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (vm.ExitAccountCommand.CanExecute(null))
                vm.ExitAccountCommand.Execute(null);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TanderDatabaseEntities db = new TanderDatabaseEntities();
            int i = 1;
            foreach (var item in db.Employees)
            {
                if (i < 10)
                    item.PhotoPath = Environment.CurrentDirectory + "\\Images\\image_00" + i.ToString() + ".jpg";
                if (i >= 10 && i < 100)
                    item.PhotoPath = Environment.CurrentDirectory + "\\Images\\image_0" + i.ToString() + ".jpg";
                if (i >= 100 && i < 1000)
                    item.PhotoPath = Environment.CurrentDirectory + "\\Images\\image_" + i.ToString() + ".jpg";
                i++;
            }
            db.SaveChanges();
            MessageBox.Show("Теперь изображения храняться здесь: " + Environment.CurrentDirectory,
                "База данных успешно обновлена",
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
