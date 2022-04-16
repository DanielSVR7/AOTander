using AOTander.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AOTander.Views
{
    public partial class LoginsWindow : Window
    {
        readonly TanderDatabaseEntities db = new TanderDatabaseEntities();
        public LoginsWindow()
        {
            InitializeComponent();
            List<Logins> loglist = db.Logins.ToList();
            LoginsDataGrid.ItemsSource = loglist;
        }
    }
}
