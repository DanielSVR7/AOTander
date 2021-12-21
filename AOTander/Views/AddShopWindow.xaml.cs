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
    /// <summary>
    /// Логика взаимодействия для AddShopWindow.xaml
    /// </summary>
    public partial class AddShopWindow : Window
    {
        public static DependencyProperty AddressProperty = DependencyProperty.Register(
            nameof(Address),
            typeof(string),
            typeof(MainWindow),
            new PropertyMetadata(null));
        public string Address { get; set; }
        public AddShopWindow()
        {
            InitializeComponent();
        }
    }
}
