using AOTander.Models;
using AOTander.ViewModels.Base;
using AOTander.Infrastructure.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AOTander.Views;
using System.Windows;
using System;
using Microsoft.Win32;

namespace AOTander.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public TanderDatabaseEntities db = new TanderDatabaseEntities();
        private Logins Login { get; set; }
        private string _TimerText;
        public string TimerText { get => _TimerText; set => Set(ref _TimerText, value); }
        private Users _User;
        public Users User { get => _User; set => Set(ref _User, value); }

        private ObservableCollection<Employees> _Employees;
        public ObservableCollection<Employees> Employees    //Свойство коллекции сотрудников
        {
            get => _Employees;
            set => Set(ref _Employees, value);
        }

        private ObservableCollection<Shops> _Shops;          //Свойство коллекции магазинов
        public ObservableCollection<Shops> Shops
        {
            get => _Shops;
            set => Set(ref _Shops, value);
        }

        public ObservableCollection<Positions> Positions { get; }   //Свойство коллекции должностей

        #region Команды

        #region LoadImageCommand - Команда загрузки изображения
        public ICommand LoadImageCommand { get; }
        private bool CanLoadImageCommandExecute(object p)
        {
            if (p is Employees employee && SelectedShop.Employees.Contains(employee))
                return true;
            else return false;
        }
        private void OnLoadImageCommandExecuted(object p)
        {
            if (!(p is Employees employee)) return;
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Файлы изображений|*.bmp;*.png;*.jpg"
            };
            if (openDialog.ShowDialog() != true)
                return;
            string path = openDialog.FileName;
            employee.PhotoPath = path;
        }
        #endregion 

        #region ExitAccountCommand - Команда выхода из аккаунта
        public ICommand ExitAccountCommand { get; }
        public Action ExitAccountAction { get; set; }
        private bool CanExitAccountCommandExecute(object p) => User != null;
        private void OnExitAccountCommandExecuted(object p)
        {
            Login.WorkingHours = DateTime.Parse(TimerText);
            db.Logins.Add(Login);
            db.SaveChanges();
            User = null;
            ExitAccountAction?.Invoke();   
        }

        #endregion

        #region AddShopCommand - Команда добавления магазина
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

        #region EditShopCommand - Команда редактирования магазина
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

        #region DeleteShopCommand - Команда удаления магазина
        public ICommand DeleteShopCommand { get; }
        private bool CanDeleteShopCommandExecute(object p)
        {
            if (p is Shops shop && Shops.Contains(shop) && shop.Employees.Count == 0)
                return true;
            else return false;
        }
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

        #region SaveEmployeesCommand - Команда сохранения сотрудников
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

        #region DeleteEmployeeCommand - Команда удаления сотрудника
        public ICommand DeleteEmployeeCommand { get; }
        private bool CanDeleteEmployeeCommandExecute(object p)
        {
            if (p is Employees employee && Employees.Contains(employee))
                return true;
            else return false;

        }
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
            #region Объявление команд

            AddShopCommand = new LambdaCommand(OnAddShopCommandExecuted, CanAddShopCommandExecute);
            EditShopCommand = new LambdaCommand(OnEditShopCommandExecuted, CanEditShopCommandExecute);
            DeleteShopCommand = new LambdaCommand(OnDeleteShopCommandExecuted, CanDeleteShopCommandExecute);
            SaveEmployeesCommand = new LambdaCommand(OnSaveEmployeesCommandExecuted, CanSaveEmployeesCommandExecute);
            DeleteEmployeeCommand = new LambdaCommand(OnDeleteEmployeeCommandExecuted, CanDeleteEmployeeCommandExecute);
            ExitAccountCommand = new LambdaCommand(OnExitAccountCommandExecuted, CanExitAccountCommandExecute);
            LoadImageCommand = new LambdaCommand(OnLoadImageCommandExecuted, CanLoadImageCommandExecute);

            #endregion 

            Shops = new ObservableCollection<Shops>(db.Shops.ToList());
            Positions = new ObservableCollection<Positions>(db.Positions.ToList());
            Employees = new ObservableCollection<Employees>(db.Employees.ToList());
        }
        public void SetUser(Users user)
        {
            User = user;
            if (User != null)
            {
                Login = new Logins
                {
                    UserID = User.Id,
                    LoginTime = DateTime.Now
                };
            }
            else
                throw new Exception("Ошибка авторизации");
        }

        private Shops _SelectedShop;
        public Shops SelectedShop                   //Свойство выбранного магазина
        {
            get => _SelectedShop;
            set => Set(ref _SelectedShop, value);
        }
        private Employees _SelectedEmployee;
        public Employees SelectedEmployee           //Свойство выбранного сотрудника
        {
            get => _SelectedEmployee;
            set => Set(ref _SelectedEmployee, value);
        }
    }
}