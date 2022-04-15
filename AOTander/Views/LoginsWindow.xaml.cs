using AOTander.Models;
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
