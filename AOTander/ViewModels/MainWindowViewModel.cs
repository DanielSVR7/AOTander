using AOTander.Models;
using AOTander.ViewModels.Base;
using AOTander.Infrastructure.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AOTander.Views;
using System.Windows;

namespace AOTander.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public DatabaseEntities db = new DatabaseEntities();

        public ObservableCollection<Employees> _Employees;
        public ObservableCollection<Employees> Employees
        {
            get => _Employees;
            set => Set(ref _Employees, value);
        }

        public ObservableCollection<Shops> _Shops;
        public ObservableCollection<Shops> Shops 
        {
            get => _Shops;
            set => Set(ref _Shops, value);
        }

        public ObservableCollection<Positions> Positions { get; }

        #region Команды

        #region AddShopCommand
        public ICommand AddShopCommand { get; }
        private bool CanAddShopCommandExecute(object p) => true;
        private void OnAddShopCommandExecuted(object p)
        {
            var shop = new Shops();
            var dlg = new EditShopWindow();
            if (dlg.ShowDialog() == true)
            {
                shop.Address = dlg.Address;
                db.Shops.Add(shop);
                Shops.Add(shop);
                db.SaveChanges();
            }
        }
        #endregion

        #region EditShopCommand
        public ICommand EditShopCommand { get; }
        private bool CanEditShopCommandExecute(object p) => p is Shops shop && Shops.Contains(shop);
        private void OnEditShopCommandExecuted(object p)
        {
            var shop = (Shops)p;
            var dlg = new EditShopWindow { Address = shop.Address };
            dlg.Owner = Application.Current.MainWindow;
            if (dlg.ShowDialog() == true)
            {
                Shops.ElementAt(Shops.IndexOf(shop)).Address = dlg.Address;
                shop.Address = dlg.Address;
                db.SaveChanges();
                Shops = new ObservableCollection<Shops>(db.Shops.ToList());
            }
        }
        #endregion

        #region DeleteShopCommand
        public ICommand DeleteShopCommand { get; }
        private bool CanDeleteShopCommandExecute(object p) => p is Shops shop && Shops.Contains(shop) && shop.Employees.Count == 0;
        private void OnDeleteShopCommandExecuted(object p)
        {
            if (!(p is Shops shop)) return;
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите удалить магазин из списка?", "Удаление магазина",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int shop_index = Shops.IndexOf(shop);
                Shops.Remove(shop);
                db.Shops.Remove(shop);
                db.SaveChanges();
                if (Shops.Count == 0)
                    return;
                if (shop_index > 0)
                    SelectedShop = Shops[shop_index - 1];
                else
                    SelectedShop = Shops[shop_index];
            }
        }
        #endregion

        #region SaveEmployeesCommand
        public ICommand SaveEmployeesCommand { get; }
        private bool CanSaveEmployeesCommandExecute(object p) => true;
        private void OnSaveEmployeesCommandExecuted(object p)
        {
            db.SaveChanges();
            Employees = new ObservableCollection<Employees>(db.Employees.ToList());
            MessageBox.Show(
                "Внесенные изменения успешно сохранены", "Успешное сохранение",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region DeleteEmployeeCommand
        public ICommand DeleteEmployeeCommand { get; }
        private bool CanDeleteEmployeeCommandExecute(object p) => p is Employees employee && Employees.Contains(employee);
        private void OnDeleteEmployeeCommandExecuted(object p)
        {
            if (!(p is Employees employee)) return;
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите удалить сотрудника из списка?", "Удаление сотрудника",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int emp_index = Employees.IndexOf(employee);
                db.Employees.Remove(employee);
                Employees.Remove(employee);
                db.SaveChanges();
                if (SelectedShop.Employees.Count == 0)
                    return;
                if (emp_index > 0)
                    SelectedEmployee = Employees[emp_index - 1];
                else
                    SelectedEmployee = Employees[emp_index];
            }

        }
        #endregion

        #endregion
        public MainWindowViewModel()
        {

            #region Команды
            AddShopCommand = new LambdaCommand(OnAddShopCommandExecuted, CanAddShopCommandExecute);
            EditShopCommand = new LambdaCommand(OnEditShopCommandExecuted, CanEditShopCommandExecute);
            DeleteShopCommand = new LambdaCommand(OnDeleteShopCommandExecuted, CanDeleteShopCommandExecute);
            SaveEmployeesCommand = new LambdaCommand(OnSaveEmployeesCommandExecuted, CanSaveEmployeesCommandExecute);
            DeleteEmployeeCommand = new LambdaCommand(OnDeleteEmployeeCommandExecuted, CanDeleteEmployeeCommandExecute);

            #endregion Команды

            Shops = new ObservableCollection<Shops>(db.Shops.ToList());
            Positions = new ObservableCollection<Positions>(db.Positions.ToList());
            Employees = new ObservableCollection<Employees>(db.Employees.ToList());
        }

        private Shops _SelectedShop;
        public Shops SelectedShop
        { 
            get => _SelectedShop;
            set => Set(ref _SelectedShop, value);
        }
        private Employees _SelectedEmployee;
        public Employees SelectedEmployee
        {
            get => _SelectedEmployee;
            set => Set(ref _SelectedEmployee, value);
        }
    }
}