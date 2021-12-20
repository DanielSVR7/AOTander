using AOTander.Models;
using AOTander.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AOTander.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public DatabaseEntities db = new DatabaseEntities();
        public ObservableCollection<Shops> Shops { get; }
        public ObservableCollection<Shops> Employees { get; }
        public ObservableCollection<Positions> Positions { get; }
        public MainWindowViewModel()
        {
            var _shops = db.Shops.ToList();
            Shops = new ObservableCollection<Shops>(_shops);
            Positions = new ObservableCollection<Positions>(db.Positions.ToList());
        }

        private Shops _SelectedShop;
        public Shops SelectedShop { get => _SelectedShop; set => Set(ref _SelectedShop, value); }
    }
}
