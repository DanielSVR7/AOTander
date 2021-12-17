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
        public MainWindowViewModel()
        {
            var l = new List<Shops>();
            foreach (var item in db.Shops)
            {
                l.Add(new Shops
                {
                    Id = item.Id,
                    Address = item.Address,
                    DirectorID = item.DirectorID
                });
            }
            Shops = new ObservableCollection<Shops>(l);
        }
    }
}
