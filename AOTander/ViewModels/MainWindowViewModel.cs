using AOTander.Models;
using AOTander.ViewModels.Base;
using AOTander.Infrastructure.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace AOTander.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public DatabaseEntities db = new DatabaseEntities();
        public ObservableCollection<Shops> Shops { get; }
        public ObservableCollection<Shops> Employees { get; }
        public ObservableCollection<Positions> Positions { get; }

        #region CreateShopCommand
        public ICommand CreateShopCommand { get; }
        private bool CanCreateShopCommandExecute(object p) => true;
        private void OnCreateShopCommandExecuted(object p)
        {
            var new_shop = new Shops
            {
                Address = "Магазин " + (Shops.Count + 1),
                Id = Shops.Last().Id + 1,
                DirectorID = 13
            };
            Shops.Add(new_shop);
        }
        #endregion

        #region DeleteShopCommand
        public ICommand DeleteShopCommand { get; }
        private bool CanDeleteShopCommandExecute(object p) => p is Shops shop && Shops.Contains(shop);
        private void OnDeleteShopCommandExecuted(object p)
        {
            if (!(p is Shops shop)) return;
            int shop_index = Shops.IndexOf(shop);
            Shops.Remove(shop);
            if (Shops.Count == 0)
                return;
            if (shop_index > 0)
                SelectedShop = Shops[shop_index - 1];
            else
                SelectedShop = Shops[shop_index];
        }
            #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CreateShopCommand = new LambdaCommand(OnCreateShopCommandExecuted, CanCreateShopCommandExecute);
            DeleteShopCommand = new LambdaCommand(OnDeleteShopCommandExecuted, CanDeleteShopCommandExecute);

            #endregion Команды
            Shops = new ObservableCollection<Shops>(db.Shops.ToList());
            Positions = new ObservableCollection<Positions>(db.Positions.ToList());
        }

        private Shops _SelectedShop;
        public Shops SelectedShop 
        { 
            get => _SelectedShop; 
            set => Set(ref _SelectedShop, value); 
        }
    }
}
