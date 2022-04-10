using AOTander.ViewModels;
using AOTander.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AOTander.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            AuthorizationWindowViewModel vm = new AuthorizationWindowViewModel();
            this.DataContext = vm;
            if (vm.LoginAction == null)
                vm.LoginAction = new Action(() => Login());
        }
        private void Login()
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((dynamic)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
        }
    }
}
