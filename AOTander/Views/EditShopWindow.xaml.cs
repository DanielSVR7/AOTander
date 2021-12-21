using System.Windows;

namespace AOTander.Views
{
    /// <summary>
    /// Логика взаимодействия для AddShopWindow.xaml
    /// </summary>
    public partial class EditShopWindow
    {
        public static DependencyProperty AddressProperty = DependencyProperty.Register(
            nameof(Address),
            typeof(string),
            typeof(EditShopWindow),
            new PropertyMetadata(null));
        public string Address
        { 
            get => (string) GetValue(AddressProperty); 
            set => SetValue(AddressProperty, value); 
        }
        public EditShopWindow() => InitializeComponent();
    }
}
